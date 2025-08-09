namespace Comparatist.View.Common
{
    internal abstract class Renderer<T> : IRenderer where T : Control
    {
        protected T Control { get; private set; }

        public Renderer(T control)
        {
            Control = control;
        }

        public virtual void Dispose() { }

        public void Show()
        {
            Control.Show();
        }

        public void Hide()
        {
            Control.Hide();
        }

        public void ShowError(string? message)
        {
            MessageBox.Show(
                    message ?? string.Empty,
                    nameof(Exception),
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

    }
}
