using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Word: IRecord, INativeRecord, ICheckableRecord, IContentEquatable<Word>
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public Guid LanguageId { get; set; }
        [Key(5)] public Guid StemId { get; set; }
        [Key(6)] public bool IsNative { get; set; } = false;
        [Key(7)] public bool IsChecked { get; set; } = false;

        public object Clone()
        {
            return new Word
            {
                Id = Id,
                Value = Value,
                Translation = Translation,
                Comment = Comment,
                LanguageId = LanguageId,
                StemId = StemId,
                IsNative = IsNative,
                IsChecked = IsChecked
            };
        }

        public bool EqualsContent(Word other)
        {
            if (other == null)
                return false;

            return Id == other.Id
                   && Value == other.Value
                   && Translation == other.Translation
                   && Comment == other.Comment
                   && IsNative == other.IsNative
                   && IsChecked == other.IsChecked
                   && StemId == other.StemId
                   && LanguageId == other.LanguageId;
        }

        public override string ToString()
        {
            return $"{Value} - {Translation}";
        }
    }
}
