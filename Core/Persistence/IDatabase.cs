namespace Comparatist
{
    public interface IDatabase
    {
        IRepository<SemanticGroup> SemanticGroups { get; }
        IRepository<Source> Sources { get; }
        IRepository<Language> Languages { get; }
        IRepository<Root> Roots { get; }
        IRepository<Stem> Stems { get; }
        IRepository<Word> Words { get; }

        void Save(string path);
        void Load(string path);
        void Clear();
    }
}
