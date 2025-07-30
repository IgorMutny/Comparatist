using MessagePack;

namespace Comparatist.Core.Records
{
    [MessagePackObject]
    public class Word: IRecord
    {
        [Key(0)] public Guid Id { get; set; }
        [Key(1)] public string Value { get; set; } = string.Empty;
        [Key(2)] public string Translation { get; set; } = string.Empty;
        [Key(3)] public string Comment { get; set; } = string.Empty;
        [Key(4)] public Guid LanguageId { get; set; }
        [Key(5)] public Guid StemId { get; set; }
        [Key(6)] public bool IsNative { get; set; } = false;
        [Key(7)] public bool IsChecked { get; set; } = false;
        [Key(8)] public bool IsDeleted { get; set; } = false;

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
                IsChecked = IsChecked,
                IsDeleted = IsDeleted
            };
        }
    }
}
