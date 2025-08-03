using Comparatist.Core.Records;
using Comparatist.Services.CategoryTree;
using Comparatist.Services.TableCache;

namespace Comparatist.Services.CategoryTableComposing
{
    internal class CategoryTableComposingService
    {
        private Dictionary<Guid, CachedSection> _sections = new();
        private List<CachedSection> _rootSections = new();
        private CachedSection _uncategorizedSection = CreateUncategorizedSection();

        public IEnumerable<CachedSection> GetTable()
        {
            var sections = new List<CachedSection>();

            foreach (var section in _rootSections)
                sections.Add((CachedSection)section.Clone());

            sections.Add((CachedSection)_uncategorizedSection.Clone());

            return sections;
        }

        public void RebuildCache(IEnumerable<CachedCategoryNode> nodes, IEnumerable<CachedSection> flatSections)
        {
            _uncategorizedSection = CreateUncategorizedSection();

            _sections.Clear();
            _rootSections.Clear();

            foreach (var node in nodes)
            {
                var rootSection = ConvertNodeToSection(node);
                _rootSections.Add(rootSection);
            }

            foreach (var section in flatSections)
            {
                foreach (var block in section.Blocks)
                {
                    if (block.Root.CategoryIds.Count > 0)
                    {
                        foreach (var categoryId in block.Root.CategoryIds)
                            _sections[categoryId].Blocks.Add(block);
                    }
                    else
                    {
                        _uncategorizedSection.Blocks.Add(block);
                    }
                }
            }
        }

        private CachedSection ConvertNodeToSection(CachedCategoryNode node)
        {
            var section = new CachedSection { Category = node.Category };

            foreach (var child in node.Children)
            {
                var childSection = ConvertNodeToSection(child);
                section.Children.Add(childSection);
            }

            _sections.Add(section.Category.Id, section);
            return section;
        }

        private static CachedSection CreateUncategorizedSection()
        {
            return new CachedSection
            {
                Category = new Category()
                {
                    Value = "Uncategorized"
                }
            };
        }
    }
}
