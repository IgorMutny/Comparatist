using Comparatist.Data.Persistence;
using MessagePack;

namespace Comparatist.Data.Entities
{
    [MessagePackObject]
    public class Stem : IRecord, IContentEquatable<Stem>
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public List<Guid> RootIds { get; set; } = new();
        [Key(6)] public bool IsChecked { get; set; } = false;

        public object Clone()
        {
            return new Stem
            {
                Id = Id,
                Value = Value,
                Translation = Translation,
                Comment = Comment,
                RootIds = [.. RootIds],
                IsChecked = IsChecked
            };
        }

        public bool EqualsContent(Stem? other)
        {
            if (other == null) 
                return false;

            return Id == other.Id
                   && Value == other.Value
                   && Translation == other.Translation
                   && Comment == other.Comment
                   && IsChecked == other.IsChecked
                   && new HashSet<Guid>(RootIds).SetEquals(other.RootIds);
        }

        public override string ToString()
        {
            return $"{Value} - {Translation}";
        }
    }
}
