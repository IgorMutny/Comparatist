using Comparatist.Data.Persistence;
using Comparatist.Data.Entities;

namespace Comparatist.Application.Cache
{
    public class CachedRoot : IDisplayableCachedRecord, IContentEquatable<CachedRoot>
    {
        public required Root Record { get; set; }
        public Dictionary<Guid, CachedStem> Stems { get; set; } = new();

        public IEnumerable<CachedStem> OrderedStems 
            => Stems.Values.OrderBy(e => e.Record.Value).ToList();

        public string Value => Record.Value;
        public string Translation => Record.Translation;
        public bool IsNative => Record.IsNative;
        public bool IsChecked => Record.IsChecked;

        public object Clone()
        {
            return new CachedRoot
            {
                Record = (Root)Record.Clone(),
                Stems = Stems
                    .Select(pair =>
                        new KeyValuePair<Guid, CachedStem>(
                            pair.Key,
                            (CachedStem)pair.Value.Clone()))
                            .ToDictionary()
            };
        }

        public bool EqualsContent(CachedRoot? other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record)
                && Stems.Keys.ToHashSet().SetEquals(other.Stems.Keys);
        }
    }
}
