using Comparatist.Core.Records;
using MessagePack;

namespace Comparatist.Core.Infrastructure
{
    [MessagePackObject]
    public class SerializableDatabase
    {
        [Key(0)] public ProjectMetadata Metadata { get; set; } = new();
        [Key(1)] public List<Category> Categories { get; set; } = new();
        [Key(2)] public List<Language> Languages { get; set; } = new();
        [Key(3)] public List<Root> Roots { get; set; } = new();
        [Key(4)] public List<Stem> Stems { get; set; } = new();
        [Key(5)] public List<Word> Words { get; set; } = new();

        public SerializableDatabase() { }

        public SerializableDatabase(Database db)
        {
            Metadata = db.Metadata;
            Categories = db.GetRepository<Category>().Export().OfType<Category>().ToList();
            Languages = db.GetRepository<Language>().Export().OfType<Language>().ToList();
            Roots = db.GetRepository<Root>().Export().OfType<Root>().ToList();
            Stems = db.GetRepository<Stem>().Export().OfType<Stem>().ToList();
            Words = db.GetRepository<Word>().Export().OfType<Word>().ToList();
        }

        public void RestoreTo(Database db)
        {
            db.Metadata.Modified = Metadata.Modified;

            db.GetRepository<Category>().Import(Categories);
            db.GetRepository<Language>().Import(Languages);
            db.GetRepository<Root>().Import(Roots);
            db.GetRepository<Stem>().Import(Stems);
            db.GetRepository<Word>().Import(Words);
        }
    }
}
