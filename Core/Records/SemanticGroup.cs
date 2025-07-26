using MessagePack;

namespace Comparatist
{
    [MessagePackObject]
    public class SemanticGroup: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public Guid? ParentId { get; set; }
        [Key(3)] public bool Deleted { get; set; } = false;
    }
}
