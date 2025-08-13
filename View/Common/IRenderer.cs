namespace Comparatist.View.Common
{
    internal interface IRenderer : IDisposable
    {
        void Show();
        void Hide();
        void OnBeginUpdate();
        void OnEndUpdate();
        void ShowError(string? message);
    }
}
