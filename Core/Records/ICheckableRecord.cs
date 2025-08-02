namespace Comparatist.Core.Records
{
    public interface ICheckableRecord: IRecord
    {
        bool IsChecked { get; set; }
    }
}
