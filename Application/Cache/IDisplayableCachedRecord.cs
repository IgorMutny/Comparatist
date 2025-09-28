namespace Comparatist.Application.Cache
{
    internal interface IDisplayableCachedRecord : ICachedRecord
    {
        string Value { get; }
        string Translation { get; }
		string Comment { get; }
		bool IsChecked { get; }
    }
}
