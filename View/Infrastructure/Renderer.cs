namespace Comparatist.View.Infrastructure
{
    internal abstract class Renderer<T> where T : Control
    {
        protected T Control { get; private set; }

        public Renderer(T control)
        {
            Control = control;
        }

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
