using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedWord: ICloneable, IContentEquatable<CachedWord>
    {
        public required Word Record { get; set; }

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
