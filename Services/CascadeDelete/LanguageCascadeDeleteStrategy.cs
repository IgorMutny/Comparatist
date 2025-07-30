using Comparatist.Core.Records;

namespace Comparatist.Services.CascadeDelete
{
    internal class LanguageCascadeDeleteStrategy : CascadeDeleteStrategy<Language>
    {
        protected override IEnumerable<IRecord> Delete(Language record)
        {
            record.IsDeleted = true;
            Database.Languages.Update(record);

            return Database.Words.GetAll()
                .Where(x => x.LanguageId == record.Id)
                .ToList();
        }
    }
}
