namespace Comparatist.View.Common
{
    internal interface IRenderer : IDisposable
    {
        void Show();
        void Hide();
        void ShowError(string? message);
    }
}
