using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedCategory: ICloneable
    {
        public required Category Record { get; set; }
        public Dictionary<Guid, CachedCategory> Children { get; set; } = new();
        public Dictionary<Guid, CachedRoot> Roots { get; set; } = new();

        public object Clone()
        {
            return Clone(new HashSet<Guid>());
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
