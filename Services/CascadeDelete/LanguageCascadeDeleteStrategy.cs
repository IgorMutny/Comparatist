using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class LanguageCascadeDeleteStrategy : CascadeDeleteStrategy<Language>
    {
        public LanguageCascadeDeleteStrategy(IDatabase database) : base(database) { }

        protected override IEnumerable<IRecord> Delete(Language record)
        {
            Database.Languages.Delete(record.Id);

            return Database.Words.GetAll()
                .Where(x => x.LanguageId == record.Id)
                .ToList();
        }
    }
}
