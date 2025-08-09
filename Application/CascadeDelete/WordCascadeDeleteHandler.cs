using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.CascadeDelete
{
    internal class WordCascadeDeleteHandler : CascadeDeleteHandler<Word>
    {
        public WordCascadeDeleteHandler(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> GetBoundedRecords(Word record)
        {
            return new List<Word>();
        }
    }
}
