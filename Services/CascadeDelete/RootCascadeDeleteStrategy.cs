using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class RootCascadeDeleteStrategy : CascadeDeleteStrategy<Root>
    {
        public RootCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Root record)
        {
            Database.Roots.Delete(record.Id);

            return Database.Stems.GetAll()
                .Where(x => x.RootIds.Contains(record.Id))
                .ToList();
        }
    }
}
