using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedLanguage : ICloneable
    {
        public required Language Record { get; set; }

        public object Clone()
        {
            return new CachedLanguage { Record = (Language)Record.Clone() };
        }
    }
}
