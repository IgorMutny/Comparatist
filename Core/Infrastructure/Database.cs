using Comparatist.Core.Records;
using MessagePack;

namespace Comparatist.Core.Infrastructure
{

    public class Database : IDatabase
    {
        public ProjectMetadata Metadata { get; } = ProjectMetadata.CreateNew();

        private Dictionary<Type, IRepository> _repositories = new()
        {
            { typeof(Category), new Repository<Category>() },
            { typeof(Language), new Repository<Language>() },
            { typeof(Root),     new Repository<Root>()     },
            { typeof(Stem),     new Repository<Stem>()     },
            { typeof(Word),     new Repository<Word>()     },
        };

        public IRepository<T> GetRepository<T>() where T : class, IRecord
        {
            if (_repositories.TryGetValue(typeof(T), out var repo) && repo is IRepository<T> typedRepo)
                return typedRepo;

            throw new InvalidOperationException($"Repository for type {typeof(T).Name} not found.");
        }

        public void Save(string path)
        {
            if (File.Exists(path))
            {
                var backupPath = path + ".bak";
                File.Copy(path, backupPath, overwrite: true);
            }

            Metadata.Modified = DateTime.UtcNow;
            var state = new SerializableDatabase(this);
            using var fs = File.Create(path);
            MessagePackSerializer.Serialize(fs, state);
        }

        public void Load(string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException();

            using var fs = File.OpenRead(path);
            var state = MessagePackSerializer.Deserialize<SerializableDatabase>(fs);
            state.RestoreTo(this);
        }

        public void Clear()
        {
            foreach (var repo in _repositories.Values)
                repo.Clear();
        }
    }
}
