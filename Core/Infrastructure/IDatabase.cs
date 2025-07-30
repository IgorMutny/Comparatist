using Comparatist.Core.Records;

namespace Comparatist.Core.Infrastructure
{
    public interface IDatabase
    {
        IRepository<Category> Categories { get; }
        IRepository<Language> Languages { get; }
        IRepository<Root> Roots { get; }
        IRepository<Stem> Stems { get; }
        IRepository<Word> Words { get; }

        void Save(string path);
        void Load(string path);
        IEnumerable<IRecord> GetAllRecords();
        void RemoveDeletedRecords();
    }
}
