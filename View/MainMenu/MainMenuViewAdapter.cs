using Comparatist.Services.Infrastructure;
using Comparatist.View.Autoreplace;
using Comparatist.View.Forms;
using Comparatist.View.Infrastructure;
using Comparatist.View.WordGrid;

namespace Comparatist.View.MainMenu
{
    internal class MainMenuViewAdapter : ViewAdapter<MenuStrip>
    {
        private const string Extension = ".comparatist";

        private Dictionary<ContentTypes, IViewAdapter> _adapters = new();
        private Dictionary<ContentTypes, ToolStripMenuItem> _adapterItems = new();
        private Dictionary<SortingTypes, ToolStripMenuItem> _sortingItems = new();
        private string _filePath = string.Empty;
        private ToolStripMenuItem _adaptersItem = new();

        public event Action<string>? LoadRequest;
        public event Action<string>? SaveRequest;
        public event Action? ExitRequest;

        public MainMenuViewAdapter(MenuStrip control) : base(control)
        {
            SetupMenu();
        }

        protected override void Unsubscribe() { }

        public void RegisterViewAdapter(ContentTypes type, IViewAdapter adapter, string text)
        {
            _adapters[type] = adapter;
            var adapterItem = AddMenuItem(text, () => ShowContent(type), _adaptersItem);
            _adapterItems[type] = adapterItem;
        }

        private void SetupMenu()
        {
            var fileItem = AddMenuItem("File", null, Control);

            var loadItem = AddMenuItem("Load...", Load, fileItem);
            var saveAsItem = AddMenuItem("Save as...", SaveAs, fileItem);
            var saveItem = AddMenuItem("Save", Save, fileItem);

            fileItem.DropDownItems.Add(new ToolStripSeparator());
            var exitItem = AddMenuItem("Exit", Exit, fileItem);

            _adaptersItem = AddMenuItem("Content", null, Control);

            var settingsItem = AddMenuItem("Settings", null, Control);

            var alphabetItem = AddMenuItem(
                "Roots by alphabet",
                () => SwitchRootSortingType(SortingTypes.Alphabet),
                settingsItem);

            var categories = AddMenuItem(
                "Roots by categories",
                () => SwitchRootSortingType(SortingTypes.Categories),
                settingsItem);

            _sortingItems.Add(SortingTypes.Alphabet, alphabetItem);
            _sortingItems.Add(SortingTypes.Categories, categories);

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
                    LoadRequest?.Invoke(_filePath);
                }
            }
        }

        private void Save()
        {
            if (string.IsNullOrEmpty(_filePath))
                SaveAs();
            else
                SaveRequest?.Invoke(_filePath);
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
                    SaveRequest?.Invoke(_filePath);
                }
            }
        }

        private void ShowContent(ContentTypes type)
        {
            foreach (var pair in _adapters)
            {
                if (pair.Key == type)
                    pair.Value.Show();
                else
                    pair.Value.Hide();
            }

            foreach (var pair in _adapterItems)
                pair.Value.Checked = pair.Key == type;
        }

        private void ShowAutoReplace()
        {
            AutoReplaceManager.Instance.ShowForm();
        }

        private void SwitchRootSortingType(SortingTypes type)
        {
            foreach (var pair in _sortingItems)
                pair.Value.Checked = pair.Key == type;

            var adapter = _adapters[ContentTypes.Words] as WordGridViewAdapter;

            if (adapter != null)
                adapter.SortingType = type;
        }

        private void Exit()
        {
            ExitRequest?.Invoke();
        }
    }
}
