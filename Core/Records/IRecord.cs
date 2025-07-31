namespace Comparatist.Core.Records
{
    public interface IRecord: ICloneable
    {
        Guid Id { get; set; }
    }
}
