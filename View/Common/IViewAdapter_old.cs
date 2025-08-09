namespace Comparatist.View.Common
{
    internal interface IViewAdapter_old: IDisposable
    {
        public event Action? RenderRequest;
        void Show();
        void Hide();
        void ShowError(string? message);
    }
}
