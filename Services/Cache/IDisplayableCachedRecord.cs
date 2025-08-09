namespace Comparatist.Services.Cache
{
    internal interface IDisplayableCachedRecord : ICachedRecord
    {
        string Value { get; }
        string Translation { get; }
        bool IsNative { get; }
        bool IsChecked { get; }
    }
}
