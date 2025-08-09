using Comparatist.Data.Entities;

namespace Comparatist.Application.CascadeDelete
{
    internal interface ICascadeDeleteHandler
    {
        IEnumerable<IRecord> GetBoundedRecords(IRecord record);
        void Delete(IRecord record);
    }
}
