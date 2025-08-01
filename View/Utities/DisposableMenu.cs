namespace Comparatist.View.Utities
{
    internal class DisposableMenu : IDisposable
    {
        private ContextMenuStrip _menu;

        private ToolStripItemClickedEventHandler _handler;

        public DisposableMenu(params (string Text, Action Action)[] items)
        {
            _menu = new ContextMenuStrip();

            foreach (var (text, action) in items)
            {
                var item = new ToolStripMenuItem(text) { Tag = action };
                _menu.Items.Add(item);
            }

            _handler = OnItemClicked;
            _menu.ItemClicked += _handler;
        }

        public void Show(Control control, Point point) => _menu.Show(control, point);

        private void OnItemClicked(object? sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == null || e.ClickedItem.Tag is not Action action)
                throw new InvalidOperationException($"Menu item '{e.ClickedItem?.Text}' must have Action as Tag.");

            action();
        }

        public void Dispose()
        {
            _menu.ItemClicked -= _handler;
            _menu.Dispose();
        }
    }
}

