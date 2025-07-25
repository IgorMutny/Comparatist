using MessagePack;

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
        void CreateNew();
    }

    [MessagePackObject]
    public class SerializableDatabase
    {
        [Key(0)] public Dictionary<Guid, SemanticGroup> SemanticGroups { get; set; } = new();
        [Key(1)] public Dictionary<Guid, Source> Sources { get; set; } = new();
        [Key(2)] public Dictionary<Guid, Language> Languages { get; set; } = new();
        [Key(3)] public Dictionary<Guid, Root> Roots { get; set; } = new();
        [Key(4)] public Dictionary<Guid, Stem> Stems { get; set; } = new();
        [Key(5)] public Dictionary<Guid, Word> Words { get; set; } = new();

        public SerializableDatabase() { }

        public SerializableDatabase(InMemoryDatabase db)
        {
            SemanticGroups = ((InMemoryRepository<SemanticGroup>)db.SemanticGroups).Export();
            Sources = ((InMemoryRepository<Source>)db.Sources).Export();
            Languages = ((InMemoryRepository<Language>)db.Languages).Export();
            Roots = ((InMemoryRepository<Root>)db.Roots).Export();
            Stems = ((InMemoryRepository<Stem>)db.Stems).Export();
            Words = ((InMemoryRepository<Word>)db.Words).Export();
        }

        public void RestoreTo(InMemoryDatabase db)
        {
            ((InMemoryRepository<SemanticGroup>)db.SemanticGroups).Import(SemanticGroups);
            ((InMemoryRepository<Source>)db.Sources).Import(Sources);
            ((InMemoryRepository<Language>)db.Languages).Import(Languages);
            ((InMemoryRepository<Root>)db.Roots).Import(Roots);
            ((InMemoryRepository<Stem>)db.Stems).Import(Stems);
            ((InMemoryRepository<Word>)db.Words).Import(Words);
        }
    }

    public class InMemoryDatabase : IDatabase
    {
        public IRepository<SemanticGroup> SemanticGroups { get; } = new InMemoryRepository<SemanticGroup>();
        public IRepository<Source> Sources { get; } = new InMemoryRepository<Source>();
        public IRepository<Language> Languages { get; } = new InMemoryRepository<Language>();
        public IRepository<Root> Roots { get; } = new InMemoryRepository<Root>();
        public IRepository<Stem> Stems { get; } = new InMemoryRepository<Stem>();
        public IRepository<Word> Words { get; } = new InMemoryRepository<Word>();

        public void Save(string path)
        {
            var state = new SerializableDatabase(this);
            using var fs = File.Create(path);
            MessagePackSerializer.Serialize(fs, state);
        }

        public void Load(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();
            using var fs = File.OpenRead(path);
            var state = MessagePackSerializer.Deserialize<SerializableDatabase>(fs);
            state.RestoreTo(this);
        }

        public void CreateNew()
        {
            SemanticGroups.Clear();
            Sources.Clear();
            Languages.Clear();
            Roots.Clear();
            Stems.Clear();
            Words.Clear();
        }
    }
}
