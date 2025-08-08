using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class RootCascadeDeleteHandler : CascadeDeleteHandler<Root>
    {
        public RootCascadeDeleteHandler(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> GetBoundedRecords(Root record)
        {
            return Database.GetRepository<Stem>().GetAll()
                .Where(x => x.RootIds.Contains(record.Id))
                .ToList();
        }
    }
}
