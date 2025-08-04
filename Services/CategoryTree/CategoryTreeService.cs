using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CategoryTree
{
    internal class CategoryTreeService
    {
        private readonly Guid NoParentId = Guid.Empty;

        private IDatabase _database;
        private List<CachedCategory> _rootNodes = new();
        private Dictionary<Guid, CachedCategory> _allNodes = new();
        private bool _isDirty = false;

        public CategoryTreeService(IDatabase database)
        {
            _database = database;
        }

        public IEnumerable<CachedCategory> GetTree()
        {
            UpdateCacheIfDirty();

            return _rootNodes.Select(n => (CachedCategory)n.Clone());
        }

        public void MarkDirty()
        {
            _isDirty = true;
        }

        public void RebuildCache()
        {
            _rootNodes.Clear();
            _allNodes.Clear();

            foreach (var category in _database.GetRepository<Category>().GetAll())
                _allNodes.Add(category.Id, new CachedCategory { Record = category });

            foreach (var node in _allNodes.Values)
            {
                if (node.Record.ParentId == node.Record.Id)
                {
                    throw new InvalidOperationException($"Category {node.Record.Id} can not be a parent for itself");
                }
                else if (node.Record.ParentId == NoParentId)
                {
                    _rootNodes.Add(node);
                }
                else
                {
                    if (!_allNodes.TryGetValue(node.Record.ParentId, out var parentNode))
                        throw new InvalidOperationException($"Category {node.Record.ParentId} not found in cache");

                    parentNode.Children.Add(node.Record.Id, node);
                }
            }

            _rootNodes = _rootNodes.OrderBy(e => e.Record.Order).ToList();

            foreach (var node in _allNodes.Values)
                if (node.Children.Count > 1)
                    node.Children = node.Children.OrderBy(child => child.Value.Record.Order).ToDictionary();
        }

        private void UpdateCacheIfDirty()
        {
            if (_isDirty)
                RebuildCache();

            _isDirty = false;
        }
    }
}
