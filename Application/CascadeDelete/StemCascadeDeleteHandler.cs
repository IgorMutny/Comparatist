using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.CascadeDelete
{
    internal class StemCascadeDeleteHandler : CascadeDeleteHandler<Stem>
    {
        public StemCascadeDeleteHandler(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> GetBoundedRecords(Stem record)
        {
            return Database.GetRepository<Word>()
                .GetAll()
                .Where(x => x.StemId == record.Id)
                .ToList();
        }
    }
}
