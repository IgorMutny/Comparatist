using Comparatist.Core.Records;
using MessagePack;

namespace Comparatist.Core.Infrastructure
{

    public class Database : IDatabase
    {
        public IRepository<Category> Categories { get; } = new Repository<Category>();
        public IRepository<Language> Languages { get; } = new Repository<Language>();
        public IRepository<Root> Roots { get; } = new Repository<Root>();
        public IRepository<Stem> Stems { get; } = new Repository<Stem>();
        public IRepository<Word> Words { get; } = new Repository<Word>();

        public void Save(string path)
        {
            if (File.Exists(path))
            {
                var backupPath = path + ".bak";
                File.Copy(path, backupPath, overwrite: true);
            }

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

        public IEnumerable<IRecord> GetAllRecords()
        {
            return Categories.GetAll()
                .Concat<IRecord>(Languages.GetAll())
                .Concat(Roots.GetAll())
                .Concat(Stems.GetAll())
                .Concat(Words.GetAll())
                .ToList();
        }

        public void RemoveDeletedRecords()
        {
            Categories.RemoveDeletedRecords();
            Languages.RemoveDeletedRecords();
            Roots.RemoveDeletedRecords();
            Stems.RemoveDeletedRecords();
            Words.RemoveDeletedRecords();
        }
    }
}
