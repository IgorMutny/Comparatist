namespace Comparatist.View.Common
{
    internal abstract class ViewAdapter_old<T>: IViewAdapter_old where T : Control
    {
        protected T Control { get; private set; }

        public event Action? RenderRequest;

        public ViewAdapter_old(T control)
        {
            Control = control;
        }

        public void Show()
        {
            RenderRequest?.Invoke();
            Control.Show();
        }

        public void Hide()
        {
            Control.Hide();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        public void ShowError(string? message)
        {
            MessageBox.Show(
                    message ?? string.Empty,
                    nameof(Exception),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        protected abstract void Unsubscribe();

        protected void RequestRender()
        {
            RenderRequest?.Invoke();
        }
    }
}
