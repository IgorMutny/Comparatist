using MessagePack;

namespace Comparatist
{
    [MessagePackObject]
    public class Language: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public bool Deleted { get; set; } = false;
    }
}
