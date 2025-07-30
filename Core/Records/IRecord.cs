namespace Comparatist.Core.Records
{
    public interface IRecord
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
