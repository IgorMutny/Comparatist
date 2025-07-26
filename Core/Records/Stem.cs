using MessagePack;

namespace Comparatist
{
    [MessagePackObject]
    public class Stem: IRecord
    {
        [Key(0)] public Guid Id {get; set;}
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public List<Guid> Roots { get; set; } = new();
        [Key(5)] public bool Checked { get; set; } = false;
        [Key(6)] public bool Deleted { get; set; } = false;
    }
}
