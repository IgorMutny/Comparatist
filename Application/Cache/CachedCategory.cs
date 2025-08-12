using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.Cache
{
    public class CachedCategory: ICachedRecord, IContentEquatable<CachedCategory>
    {
        public required Category Record { get; set; }
        public Dictionary<Guid, CachedCategory> Children { get; set; } = new();
        public Dictionary<Guid, CachedRoot> Roots { get; set; } = new();

        public IEnumerable<CachedRoot> OrderedRoots 
            => Roots.Values.OrderBy(e => e.Record.Value);

        public IEnumerable<CachedCategory> OrderedChildren
            => Children.Values.OrderBy(e => e.Record.Order);

        public object Clone()
        {
            return Clone(new HashSet<Guid>());
        }

        public bool EqualsContent(CachedCategory? other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record)
                && Children.Keys.ToHashSet().SetEquals(other.Children.Keys)
                && Roots.Keys.ToHashSet().SetEquals(other.Roots.Keys);
        }

        private CachedCategory Clone(HashSet<Guid> visited)
        {
            if (!visited.Add(Record.Id))
                throw new InvalidOperationException("Cycle detected during cloning");

            return new CachedCategory
            {
                Record = (Category)Record.Clone(),
                Children = Children
                    .Select(pair =>
                        new KeyValuePair<Guid, CachedCategory>(
                            pair.Key,
                            (CachedCategory)pair.Value.Clone()))
                            .ToDictionary(),
                Roots = Roots
                    .Select(pair =>
                        new KeyValuePair<Guid, CachedRoot>(
                            pair.Key,
                            (CachedRoot)pair.Value.Clone()))
                            .ToDictionary()
            };
        }
    }
}
