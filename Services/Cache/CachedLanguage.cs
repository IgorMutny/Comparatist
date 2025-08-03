using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedLanguage : ICloneable
    {
        public required Language Record;

        public object Clone()
        {
            return new CachedLanguage { Record = (Language)Record.Clone() };
        }
    }
}
