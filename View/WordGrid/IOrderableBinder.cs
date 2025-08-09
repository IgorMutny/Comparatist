namespace Comparatist.View.WordGrid
{
    internal interface IOrderableBinder
    {
        bool NeedsReorder { get; set; }
        string Order { get; }
    }
}
