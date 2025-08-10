using Comparatist.Data.Entities;

namespace Comparatist.Application.Cache
{
    public class CachedLanguage : ICachedRecord, IContentEquatable<CachedLanguage>
    {
        public required Language Record { get; set; }

        public object Clone()
        {
            return new CachedLanguage { Record = (Language)Record.Clone() };
        }

        public bool EqualsContent(CachedLanguage? other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record);
        }
    }
}
