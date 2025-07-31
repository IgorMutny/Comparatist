using Comparatist.Core.Records;

namespace Comparatist.Services.CategoryTree
{
    public class CachedCategoryNode : ICloneable
    {
        public required Category Category;
        public List<CachedCategoryNode> Children = new();

        public object Clone()
        {
            return Clone(new HashSet<Guid>());
        }

        private CachedCategoryNode Clone(HashSet<Guid> visited)
        {
            if (!visited.Add(Category.Id))
                throw new InvalidOperationException("Cycle detected during cloning");

            return new CachedCategoryNode
            {
                Category = (Category)Category.Clone(),
                Children = Children.Select(c => c.Clone(visited)).ToList()
            };
        }
    }
}
