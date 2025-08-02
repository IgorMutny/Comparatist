using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Root: IRecord, INativeRecord, ICheckableRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public List<Guid> CategoryIds { get; set; } = new();
        [Key(5)] public bool IsNative { get; set; } = false;
        [Key(6)] public bool IsChecked { get; set; } = false;

        public object Clone()
        {
            return new Root
            {
                Id = Id,
                Value = Value,
                Translation = Translation,
                Comment = Comment,
                CategoryIds = CategoryIds,
                IsNative = IsNative,
                IsChecked = IsChecked
            };
        }
    }
}
