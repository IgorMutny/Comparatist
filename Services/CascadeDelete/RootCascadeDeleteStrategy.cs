using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class RootCascadeDeleteStrategy : CascadeDeleteStrategy<Root>
    {
        protected override IEnumerable<IRecord> Delete(Root record)
        {
            record.IsDeleted = true;
            Database.Roots.Update(record);

            return Database.Stems.GetAll()
                .Where(x => x.RootIds.Contains(record.Id))
                .ToList();
        }
    }
}
