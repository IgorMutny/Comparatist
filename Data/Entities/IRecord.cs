namespace Comparatist.Data.Entities
{
    public interface IRecord: ICloneable
    {
        Guid Id { get; set; }
        string Value { get; set; }
    }
}
