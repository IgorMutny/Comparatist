namespace Comparatist.Application.Cache
{
    internal interface IDisplayableCachedRecord : ICachedRecord
    {
        string Value { get; }
        string Translation { get; }
        bool IsNative { get; }
        bool IsChecked { get; }
    }
}
