using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class ProjectCache
    {
        public readonly Dictionary<Guid, CachedLanguage> Languages = new();
        public readonly Dictionary<Guid, CachedCategory> Categories = new();
        public readonly Dictionary<Guid, CachedRoot> Roots = new();
        public readonly Dictionary<Guid, CachedStem> Stems = new();
        public readonly Dictionary<Guid, CachedWord> Words = new();

        public readonly HashSet<Guid> BaseCategoryIds = new();
        public readonly HashSet<Guid> UncategorizedRoots = new();
    }
}
