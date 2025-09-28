using Comparatist.Application.Management;
using Comparatist.Data.Persistence;
using Comparatist.View.Autoreplace;
using Comparatist.View.Common;
using Comparatist.View.Stats;
using Comparatist.View.WordGrid;
using System.Text;

namespace Comparatist.View.MainMenu
{
    internal class MainMenuHandler
    {
        private const string Extension = ".comparatist";

        private Dictionary<ContentTypes, IPresenter> _presenters = new();
        private Dictionary<ContentTypes, ToolStripMenuItem> _contentItems = new();
        private Dictionary<SortingTypes, ToolStripMenuItem> _sortingTypeItems = new();

        private IProjectService _service;
        private string _filePath = string.Empty;
        private MenuStrip _menu;
        private ToolStripMenuItem _viewMenu = new();

        public event Action? ExitRequest;

        public MainMenuHandler(IProjectService service, MenuStrip menu)
        {
            _service = service;
            _menu = menu;
            SetupMenu();
        }

        public void Initialize()
        {
            SwitchRootSortingType(SortingTypes.Alphabet);
            ShowContent(ContentTypes.Words);
        }

        public void RegisterPresenter(ContentTypes type, IPresenter presenter, string text)
        {
            _presenters[type] = presenter;
            var adapterItem = AddMenuItem(text, () => ShowContent(type), _viewMenu);
            _contentItems[type] = adapterItem;
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(_filePath))
                SaveAs();
            else
                _service.SaveDatabase(_filePath);
        }

        private void SetupMenu()
        {
            var fileItem = AddMenuItem("File", null, _menu);

            var loadItem = AddMenuItem("Load...", Load, fileItem);
            var saveAsItem = AddMenuItem("Save as...", SaveAs, fileItem);
            var saveItem = AddMenuItem("Save", Save, fileItem);

            fileItem.DropDownItems.Add(new ToolStripSeparator());
            var exitItem = AddMenuItem("Exit", Exit, fileItem);

			_viewMenu = AddMenuItem("View", null, _menu);

            var alphabetItem = AddMenuItem(
                "Roots by alphabet",
                () => SwitchRootSortingType(SortingTypes.Alphabet),
				_viewMenu);

            var categories = AddMenuItem(
                "Roots by categories",
                () => SwitchRootSortingType(SortingTypes.Categories),
				_viewMenu);

            _sortingTypeItems.Add(SortingTypes.Alphabet, alphabetItem);
            _sortingTypeItems.Add(SortingTypes.Categories, categories);

            _viewMenu.DropDownItems.Add(new ToolStripSeparator());

            var expandAll = AddMenuItem(
                "Expand all",
                ExpandAll,
                _viewMenu);

			var collapseAll = AddMenuItem(
	            "Collapse all",
	            CollapseAll,
	            _viewMenu);

			_viewMenu.DropDownItems.Add(new ToolStripSeparator());

			var settingsItem = AddMenuItem("Settings", null, _menu);
            var stats = AddMenuItem("Statistics", ShowStats, settingsItem);
			var autoreplaceItem = AddMenuItem("Autoreplace settings", ShowAutoReplace, settingsItem);
        }

        private ToolStripMenuItem AddMenuItem(string text, Action? action, ToolStripMenuItem parent)
        {
            var item = new ToolStripMenuItem(text);

            if (action != null)
                item.Click += (_, _) => action();

            parent.DropDownItems.Add(item);
            return item;
        }

        private ToolStripMenuItem AddMenuItem(string text, Action? action, MenuStrip parent)
        {
            var item = new ToolStripMenuItem(text);

            if (action != null)
                item.Click += (_, _) => action();

            parent.Items.Add(item);
            return item;
        }

        private void Load()
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Select Comparatist DataBase file";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    _filePath = dialog.FileName;
                    _service.LoadDatabase(_filePath);
                    SwitchRootSortingType(SortingTypes.Alphabet);
                    ShowContent(ContentTypes.Words);
                }
            }
        }

        private void SaveAs()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = $"Comparatist DataBase files (*{Extension})|*{Extension}";
                dialog.Title = "Save Comparatist DataBase as...";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    string path = dialog.FileName;
                    _filePath = path;
                    _service.SaveDatabase(_filePath);
                }
            }
        }

        private void ShowContent(ContentTypes type)
        {
            foreach (var pair in _presenters)
            {
                if (pair.Key == type)
                    pair.Value.Show();
                else
                    pair.Value.Hide();
            }

            foreach (var pair in _contentItems)
                pair.Value.Checked = pair.Key == type;
        }

        private void ShowAutoReplace()
        {
            AutoReplaceManager.Instance.ShowForm();
        }

        private void SwitchRootSortingType(SortingTypes type)
        {
            foreach (var pair in _sortingTypeItems)
                pair.Value.Checked = pair.Key == type;

            var presenter = _presenters[ContentTypes.Words] as WordGridPresenter;

            if (presenter != null)
                presenter.SortingType = type;
        }

        private void ExpandAll()
        {
			var presenter = _presenters[ContentTypes.Words] as WordGridPresenter;
            presenter?.ExpandAll();
		}

        private void CollapseAll()
        {
			var presenter = _presenters[ContentTypes.Words] as WordGridPresenter;
			presenter?.CollapseAll();
		}

        private void ShowStats()
        {
			var metadataQuery = _service.GetProjectMetadata();

            if(!metadataQuery.IsSuccess || metadataQuery.Value == null)
                return;

			var statsQuery = _service.GetProjectStats();

			if(!statsQuery.IsSuccess || statsQuery.Value == null)
				return;

			StatsManager.ShowForm(metadataQuery.Value, statsQuery.Value);
        }

        private void Exit()
        {
            ExitRequest?.Invoke();
        }
    }
}
