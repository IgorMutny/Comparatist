namespace Comparatist.View.Common
{
    internal interface IPresenter
    {
        bool IsActive { get; }

        void Show();
        void Hide();
        void RedrawAll();
    }
}
