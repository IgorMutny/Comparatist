namespace Comparatist.Core.Records
{
    public interface IOrderableRecord: IRecord
    {
        int Order { get; set; }
    }
}
