using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Category: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public Guid? ParentId { get; set; }
        [Key(3)] public bool IsDeleted { get; set; } = false;
    }
}
