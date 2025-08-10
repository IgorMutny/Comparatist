using Comparatist.View.Common;

namespace Comparatist.View.WordGrid.Binders
{
    internal interface ICompositeBinder: IBinder
    {
        void RemoveAllChildren();
    }
}
