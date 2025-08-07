using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedWord: ICloneable
    {
        public required Word Record { get; set; }

        public object Clone()
        {
            return new CachedWord { Record = (Word)Record.Clone() };
        }
    }
}
