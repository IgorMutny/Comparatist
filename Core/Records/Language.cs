using Comparatist.Core.Infrastructure;
using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Language: IRecord, IContentEquatable<Language>
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public int Order { get; set; } = 0;

        public object Clone()
        {
            return new Language
            {
                Id = Id,
                Value = Value,
                Order = Order
            };
        }

        public bool EqualsContent(Language? other)
        {
            if (other == null)
                return false;

            return Id == other.Id
                && Value == other.Value
                && Order == other.Order;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
