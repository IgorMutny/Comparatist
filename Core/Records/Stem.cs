using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Stem: IRecord
    {
        [Key(0)] public Guid Id {get; set;}
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public List<Guid> RootIds { get; set; } = new();
        [Key(5)] public bool IsNative { get; set; } = false;
        [Key(6)] public bool IsChecked { get; set; } = false;
        [Key(7)] public bool IsDeleted { get; set; } = false;

        public object Clone()
        {
            return new Stem
            {
                Id = Id,
                Value = Value,
                Translation = Translation,
                Comment = Comment,
                RootIds = RootIds,
                IsNative = IsNative,
                IsChecked = IsChecked,
                IsDeleted = IsDeleted
            };
        }
    }
}
