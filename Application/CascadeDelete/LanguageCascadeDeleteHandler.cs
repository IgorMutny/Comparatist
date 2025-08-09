using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.CascadeDelete
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
