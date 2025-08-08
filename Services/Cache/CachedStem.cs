using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedStem : ICloneable, IContentEquatable<CachedStem>
    {
        public required Stem Record { get; set; }
        public Dictionary<Guid, CachedWord> Words { get; set; } = new();

        public object Clone()
        {
            return new CachedStem
            {
                Record = (Stem)Record.Clone(),
                Words = Words
                    .Select(pair =>
                        new KeyValuePair<Guid, CachedWord>(
                            pair.Key,
                            (CachedWord)pair.Value.Clone()))
                            .ToDictionary()
            };
        }

        public bool EqualsContent(CachedStem other)
        {
            if (other == null)
                return false;

            return Record.EqualsContent(other.Record)
                 && Words.Keys.ToHashSet().SetEquals(other.Words.Keys);
        }
    }
}

