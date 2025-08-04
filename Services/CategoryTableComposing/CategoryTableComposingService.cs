using Comparatist.Core.Records;
using Comparatist.Services.Cache;

namespace Comparatist.Services.CategoryTableComposing
{
    internal class CategoryTableComposingService
    {
        private Dictionary<Guid, CachedCategory> _sections = new();
        private List<CachedCategory> _rootSections = new();
        private CachedCategory _uncategorizedSection = CreateUncategorizedSection();

        public IEnumerable<CachedCategory> GetTable()
        {
            var sections = new List<CachedCategory>();

            foreach (var section in _rootSections)
                sections.Add((CachedCategory)section.Clone());

            sections.Add((CachedCategory)_uncategorizedSection.Clone());

            return sections;
        }

        public void RebuildCache(IEnumerable<CachedCategory> nodes, IEnumerable<CachedCategory> flatSections)
        {
            _uncategorizedSection = CreateUncategorizedSection();

            _sections.Clear();
            _rootSections.Clear();

            foreach (var node in nodes)
                _rootSections.Add(node);

            foreach (var node in nodes)
                AddNode(node);

            foreach (var section in flatSections)
            {
                foreach (var block in section.Roots)
                {
                    if (block.Value.Record.CategoryIds.Count > 0)
                    {
                        foreach (var categoryId in block.Value.Record.CategoryIds)
                            _sections[categoryId].Roots.Add(block.Key, block.Value);
                    }
                    else
                    {
                        _uncategorizedSection.Roots.Add(block.Key, block.Value);
                    }
                }
            }
        }

        private void AddNode(CachedCategory category)
        {
            _sections.Add(category.Record.Id, category);

            foreach (var child in category.Children.Values)
                AddNode(child);
        }

        private static CachedCategory CreateUncategorizedSection()
        {
            return new CachedCategory
            {
                Record = new Category()
                {
                    Value = "Uncategorized"
                }
            };
        }
    }
}
