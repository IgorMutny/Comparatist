using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class StemCascadeDeleteStrategy : CascadeDeleteStrategy<Stem>
    {
        public StemCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Stem record)
        {
            record.IsDeleted = true;
            Database.Stems.Update(record);

            return Database.Words.GetAll()
                .Where(x => x.StemId == record.Id)
                .ToList();
        }
    }
}
