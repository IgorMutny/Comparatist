using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedWord: IDisplayableCachedRecord, IContentEquatable<CachedWord>
    {
        public required Word Record { get; set; }

        public string Value => Record.Value;
        public string Translation => Record.Translation;
        public bool IsNative => Record.IsNative;
        public bool IsChecked => Record.IsChecked;

        public object Clone()
        {
            return new CachedWord { Record = (Word)Record.Clone() };
        }

        public bool EqualsContent(CachedWord? other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record);
        }
    }
}
