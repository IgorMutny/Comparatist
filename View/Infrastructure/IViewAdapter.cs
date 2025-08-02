namespace Comparatist.View.Infrastructure
{
    internal interface IViewAdapter: IDisposable
    {
        public event Action? RenderRequest;
        void Show();
        void Hide();
        void ShowError(string? message);
    }
}
