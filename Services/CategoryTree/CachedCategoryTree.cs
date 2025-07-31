namespace Comparatist.Services.CategoryTree
{
    internal class CachedCategoryTree
    {
        public List<CachedCategoryNode> RootNodes = new();
        public Dictionary<Guid, CachedCategoryNode> AllNodes = new();
    }
}
