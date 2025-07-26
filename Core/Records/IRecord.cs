namespace Comparatist
{
    public interface IRecord
    {
        Guid Id { get; set; }
        bool Deleted { get; set; }
    }
}
