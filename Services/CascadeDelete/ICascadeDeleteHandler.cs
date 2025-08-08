using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal interface ICascadeDeleteHandler
    {
        IEnumerable<IRecord> GetBoundedRecords(IRecord record);
        void Delete(IRecord record);
    }
}
