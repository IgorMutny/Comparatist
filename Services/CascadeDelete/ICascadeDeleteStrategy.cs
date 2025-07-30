using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal interface ICascadeDeleteStrategy
    {
        void SetDatabase(IDatabase database);
        IEnumerable<IRecord> Delete(IRecord record);
    }
}
