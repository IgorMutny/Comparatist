using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Category: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public Guid ParentId { get; set; } = Guid.Empty;

        public object Clone()
        {
            return new Category
            {
                Id = Id,
                Value = Value,
                ParentId = ParentId
            };
        }
    }
}
