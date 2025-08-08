using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
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
