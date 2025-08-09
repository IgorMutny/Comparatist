namespace Comparatist.Core.Infrastructure
{
    public interface IContentEquatable<T>
    {
        bool EqualsContent(T? other);
    }
}
