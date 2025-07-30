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
            Categories = ((Repository<Category>)db.Categories).Export();
            Languages = ((Repository<Language>)db.Languages).Export();
            Roots = ((Repository<Root>)db.Roots).Export();
            Stems = ((Repository<Stem>)db.Stems).Export();
            Words = ((Repository<Word>)db.Words).Export();
        }

        public void RestoreTo(Database db)
        {
            ((Repository<Category>)db.Categories).Import(Categories);
            ((Repository<Language>)db.Languages).Import(Languages);
            ((Repository<Root>)db.Roots).Import(Roots);
            ((Repository<Stem>)db.Stems).Import(Stems);
            ((Repository<Word>)db.Words).Import(Words);
        }
    }
}
