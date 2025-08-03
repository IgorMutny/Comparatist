using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class StemCascadeDeleteStrategy : CascadeDeleteStrategy<Stem>
    {
        public StemCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Stem record)
        {
            Database.GetRepository<Stem>().Delete(record.Id);

            return Database.GetRepository<Word>().GetAll()
                .Where(x => x.StemId == record.Id)
                .ToList();
        }
    }
}
