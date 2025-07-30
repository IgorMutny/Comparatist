using Comparatist.Core.Records;

namespace Comparatist.View.Tags
{
    public class WordTag
    {
        public readonly Guid Id;
        public readonly string Value;
        public readonly string Translation;
        public readonly string Comment;
        public readonly Guid LanguageId;
        public readonly Guid StemId;
        public readonly bool IsNative;
        public readonly bool IsChecked;

        public WordTag()
        {
            Id = default;
            Value = string.Empty;
            Translation = string.Empty;
            Comment = string.Empty;
            IsNative = false;
            IsChecked = false;
            LanguageId = default;
            StemId = default;
        }

        public WordTag(Guid id,
            string value,
            string translation,
            string comment,
            bool isNative,
            bool isChecked,
            Guid languageId,
            Guid stemId)
        {
            Id = id;
            Value = value;
            Translation = translation;
            Comment = comment;
            IsNative = isNative;
            IsChecked = isChecked;
            LanguageId = languageId;
            StemId = stemId;
        }

        public WordTag(Word word)
        {
            Id = word.Id;
            Value = word.Value;
            Translation = word.Translation;
            Comment = word.Comment;
            LanguageId = word.LanguageId;
            StemId = word.StemId;
            IsNative = word.IsNative;
            IsChecked = word.IsChecked;
        }
    }

}
