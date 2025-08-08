using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
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
