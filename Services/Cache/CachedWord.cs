using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedWord
    {
        public required Word Record;

        public object Clone()
        {
            return new CachedWord { Record = (Word)Record.Clone() };
        }
    }
}
