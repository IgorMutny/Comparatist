namespace Comparatist.Data.Entities
{
    public interface IContentEquatable<T>
    {
        bool EqualsContent(T? other);
    }
}
