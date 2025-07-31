namespace Comparatist.View.Infrastructure
{
    internal abstract class ViewAdapter: IDisposable
    {
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
    }
}
