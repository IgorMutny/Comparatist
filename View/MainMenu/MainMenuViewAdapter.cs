using Comparatist.View.Infrastructure;

namespace Comparatist.View.MainMenu
{
    internal class MainMenuViewAdapter : ViewAdapter<MenuStrip>
    {
        private const string Extension = ".comparatist";

        private Dictionary<ContentTypes, IViewAdapter> _adapters = new();
        private Dictionary<ContentTypes, ToolStripMenuItem> _adapterItems = new();
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

        private void Exit()
        {
            ExitRequest?.Invoke();
        }
    }
}
