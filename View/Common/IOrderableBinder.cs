namespace Comparatist.View.Common
{
    internal interface IOrderableBinder: IBinder
    {
        bool NeedsReorder { get; set; }
        IComparable Order { get; }
    }
}
