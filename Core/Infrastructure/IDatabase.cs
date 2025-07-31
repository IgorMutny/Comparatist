using Comparatist.Core.Records;

namespace Comparatist.Core.Infrastructure
{
    public interface IDatabase
    {
        ProjectMetadata Metadata { get; }
        IRepository<Category> Categories { get; }
        IRepository<Language> Languages { get; }
        IRepository<Root> Roots { get; }
        IRepository<Stem> Stems { get; }
        IRepository<Word> Words { get; }

        void Save(string path);
        void Load(string path);
        IEnumerable<IRecord> GetAllRecords();
    }
}
