using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class WordCascadeDeleteStrategy : CascadeDeleteStrategy<Word>
    {
        public WordCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Word record)
        {
            Database.Words.Delete(record.Id);
            return new List<Word>();
        }
    }
}
