using Comparatist.Core.Infrastructure;
using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Category: IRecord, IContentEquatable<Category>
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public Guid ParentId { get; set; } = Guid.Empty;
        [Key(3)] public int Order { get; set; } = 0;

        public object Clone()
        {
            return new Category
            {
                Id = Id,
                Value = Value,
                ParentId = ParentId,
                Order = Order
            };
        }

        public bool EqualsContent(Category? other)
        {
            if (other == null)
                return false;

            return Id == other.Id
                && Value == other.Value
                && ParentId == other.ParentId
                && Order == other.Order;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
