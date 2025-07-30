using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedRoot
    {
        public required Root Root;
        public readonly List<CachedStem> Stems = new();
    }
}
