using Comparatist.Core.Infrastructure;
using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedRoot : ICachedRecord, IContentEquatable<CachedRoot>
    {
        public required Root Record { get; set; }
        public Dictionary<Guid, CachedStem> Stems { get; set; } = new();

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
