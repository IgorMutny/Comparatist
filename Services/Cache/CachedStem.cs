using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedStem
    {
        public required Stem Stem;
        public readonly Dictionary<Guid, Word?> WordsByLanguage = new();
    }
}
