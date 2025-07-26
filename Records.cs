using MessagePack;

namespace Comparatist
{
    public interface IRecord
    {
        bool Deleted { get; set; }
    }

    [MessagePackObject]
    public class SemanticGroup: IRecord
    {
        [Key(0)] public string Value { get; set; } = string.Empty;
        [Key(1)] public Guid? ParentId { get; set; }
        [Key(2)] public bool Deleted { get; set; } = false;
    }

    [MessagePackObject]
    public class Source: IRecord
    {
        [Key(0)] public string Value { get; set; } = string.Empty;
        [Key(1)] public bool Deleted { get; set; } = false;
    }

    [MessagePackObject]
    public class Language: IRecord
    {
        [Key(0)] public string Value { get; set; } = string.Empty;
        [Key(1)] public bool Deleted { get; set; } = false;
    }

    [MessagePackObject]
    public class Root: IRecord
    {
        [Key(0)] public SemanticGroup[] SemanticGroups { get; set; } = Array.Empty<SemanticGroup>();
        [Key(1)] public Source Source { get; set; } = new Source();
        [Key(2)] public string Value { get; set; } = string.Empty;
        [Key(3)] public string Translation { get; set; } = string.Empty;
        [Key(4)] public string Comment { get; set; } = string.Empty;
        [Key(5)] public bool Checked { get; set; } = false;
        [Key(6)] public bool Deleted { get; set; } = false;
    }

    [MessagePackObject]
    public class Stem: IRecord
    {
        [Key(0)] public SemanticGroup[] SemanticGroups { get; set; } = Array.Empty<SemanticGroup>();
        [Key(1)] public Root[] Roots { get; set; } = Array.Empty<Root>();
        [Key(2)] public string Value { get; set; } = string.Empty;
        [Key(3)] public string Translation { get; set; } = string.Empty;
        [Key(4)] public string Comment { get; set; } = string.Empty;
        [Key(5)] public bool Checked { get; set; } = false;
        [Key(6)] public bool Deleted { get; set; } = false;
    }

    [MessagePackObject]
    public class Word: IRecord
    {
        [Key(0)] public SemanticGroup[] SemanticGroups { get; set; } = Array.Empty<SemanticGroup>();
        [Key(1)] public Stem Stem { get; set; } = new Stem();
        [Key(2)] public Language Language { get; set; } = new Language();
        [Key(3)] public string Value { get; set; } = string.Empty;
        [Key(4)] public string Translation { get; set; } = string.Empty;
        [Key(5)] public string Comment { get; set; } = string.Empty;
        [Key(6)] public bool Checked { get; set; } = false;
        [Key(7)] public bool Deleted { get; set; } = false;
    }
}
