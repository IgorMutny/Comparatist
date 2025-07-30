namespace Comparatist.Services.Cache
{
    public class DataCache
    {
        public readonly Dictionary<Guid, CachedRoot> Roots = new();
        public readonly Dictionary<Guid, CachedStem> Stems = new();
    }
}
