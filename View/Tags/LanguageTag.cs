using Comparatist.Core.Records;

namespace Comparatist.View.Tags
{
    public class LanguageTag
    {
        public readonly Guid Id;
        public readonly string Value;

        public LanguageTag(Language language)
        {
            Id = language.Id;
            Value = language.Value;
        }
    }
}
