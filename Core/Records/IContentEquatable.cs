namespace Comparatist.Core.Records
{
    public interface IContentEquatable<T>
    {
        bool EqualsContent(T other);
    }
}
