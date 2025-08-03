using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.CategoryTree
{
    internal class CategoryTreeService
    {
        private readonly Guid NoParentId = Guid.Empty;

        private IDatabase _database;
        private CachedCategoryTree _cache;
        private bool _isDirty = false;

        public CategoryTreeService(IDatabase database)
        {
            _database = database;
            _cache = new();
        }

        public IEnumerable<CachedCategoryNode> GetTree()
        {
            UpdateCacheIfDirty();

            return _cache.RootNodes.Select(n => (CachedCategoryNode)n.Clone());
        }

        public void MarkDirty()
        {
            _isDirty = true;
        }

        public void RebuildCache()
        {
            _cache.RootNodes.Clear();
            _cache.AllNodes.Clear();

            foreach (var category in _database.GetRepository<Category>().GetAll())
                _cache.AllNodes.Add(category.Id, new CachedCategoryNode { Category = category });

            foreach (var node in _cache.AllNodes.Values)
            {
                if (node.Category.ParentId == node.Category.Id)
                {
                    throw new InvalidOperationException($"Category {node.Category.Id} can not be a parent for itself");
                }
                else if (node.Category.ParentId == NoParentId)
                {
                    _cache.RootNodes.Add(node);
                }
                else
                {
                    if (!_cache.AllNodes.TryGetValue(node.Category.ParentId, out var parentNode))
                        throw new InvalidOperationException($"Category {node.Category.ParentId} not found in cache");

                    parentNode.Children.Add(node);
                }
            }

            _cache.RootNodes = _cache.RootNodes.OrderBy(e => e.Category.Order).ToList();

            foreach (var node in _cache.AllNodes.Values)
                if (node.Children.Count > 1)
                    node.Children = node.Children.OrderBy(child => child.Category.Order).ToList();
        }

        private void UpdateCacheIfDirty()
        {
            if (_isDirty)
                RebuildCache();

            _isDirty = false;
        }
    }
}
