namespace Comparatist.Core.Records
{
    public interface IRecord: ICloneable
    {
        Guid Id { get; set; }
        bool IsDeleted { get; set; }
    }
}
