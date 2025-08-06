using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class ProjectCache
    {
        public Dictionary<Guid, CachedLanguage> Languages { get; } = new();
        public Dictionary<Guid, CachedCategory> Categories { get; } = new();
        public Dictionary<Guid, CachedRoot> Roots { get; } = new();
        public Dictionary<Guid, CachedStem> Stems { get; } = new();
        public Dictionary<Guid, CachedWord> Words { get; } = new();

        public HashSet<Guid> BaseCategoryIds { get; } = new();
        public HashSet<Guid> UncategorizedRootIds { get; } = new();

        public void Clear()
        {
            Languages.Clear();
            Categories.Clear();
            Roots.Clear();
            Stems.Clear();
            Words.Clear();
            BaseCategoryIds.Clear();
            UncategorizedRootIds.Clear();
        }
    }
}
