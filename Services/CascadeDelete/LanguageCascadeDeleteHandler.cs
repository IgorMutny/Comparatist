using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class LanguageCascadeDeleteHandler : CascadeDeleteHandler<Language>
    {
        public LanguageCascadeDeleteHandler(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> GetBoundedRecords(Language record)
        {
            return Database.GetRepository<Word>().GetAll()
                .Where(x => x.LanguageId == record.Id)
                .ToList();
        }
    }
}
