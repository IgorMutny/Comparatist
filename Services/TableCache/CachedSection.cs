using Comparatist.Core.Records;

namespace Comparatist.Services.TableCache
{
    public class CachedSection : ICloneable
    {
        public required Category Category;
        public List<CachedSection> Children = new();
        public List<CachedBlock> Blocks = new();

        public object Clone()
        {
            return Clone(new HashSet<Guid>());
        }

        private CachedSection Clone(HashSet<Guid> visited)
        {
            if (!visited.Add(Category.Id))
                throw new InvalidOperationException("Cycle detected during cloning");

            return new CachedSection
            {
                Category = (Category)Category.Clone(),
                Children = Children.Select(c => c.Clone(visited)).ToList(),
                Blocks = Blocks.Select(c => (CachedBlock)c.Clone()).ToList()
            };
        }
    }
}
