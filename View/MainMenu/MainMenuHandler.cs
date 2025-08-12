using Comparatist.Application.Management;
using Comparatist.Data.Persistence;
using Comparatist.View.Autoreplace;
using Comparatist.View.Common;
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
        private ToolStripMenuItem _contentMenu = new();

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
            var adapterItem = AddMenuItem(text, () => ShowContent(type), _contentMenu);
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

            _contentMenu = AddMenuItem("Content", null, _menu);

            var settingsItem = AddMenuItem("Settings", null, _menu);

            var alphabetItem = AddMenuItem(
                "Roots by alphabet",
                () => SwitchRootSortingType(SortingTypes.Alphabet),
                settingsItem);

            var categories = AddMenuItem(
                "Roots by categories",
                () => SwitchRootSortingType(SortingTypes.Categories),
                settingsItem);

            _sortingTypeItems.Add(SortingTypes.Alphabet, alphabetItem);
            _sortingTypeItems.Add(SortingTypes.Categories, categories);

            settingsItem.DropDownItems.Add(new ToolStripSeparator());
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

                    var metadataQuery = _service.GetProjectMetadata();

                    if (metadataQuery.IsSuccess && metadataQuery.Value != null)
                        ShowMetadata(metadataQuery.Value);
                }
            }
        }

        private void ShowMetadata(ProjectMetadata data)
        {
            var builder = new StringBuilder()
                .AppendLine("Database loaded")
                .AppendLine($"Project id: {data.Id}")
                .AppendLine($"Database version: {data.Version}")
                .AppendLine($"Created: {data.Created}")
                .AppendLine($"Modified: {data.Modified}");

            MessageBox.Show( builder.ToString() );
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

        private void Exit()
        {
            ExitRequest?.Invoke();
        }
    }
}
