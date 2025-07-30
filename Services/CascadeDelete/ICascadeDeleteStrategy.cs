using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal interface ICascadeDeleteStrategy
    {
        IEnumerable<IRecord> Delete(IRecord record);
    }
}
