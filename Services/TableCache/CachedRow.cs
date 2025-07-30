using Comparatist.Core.Records;

namespace Comparatist.Services.TableCache
{
    public class CachedRow : ICloneable
    {
        public required Stem Stem;
        public Dictionary<Guid, Word?> Cells = new();

        public object Clone()
        {
            return new CachedRow
            {
                Stem = (Stem)Stem.Clone(),
                Cells = Cells
                    .Select(pair => new KeyValuePair<Guid, Word?>(
                        pair.Key,
                        pair.Value == null ? null : (Word)pair.Value.Clone()))
            .ToDictionary()
            };
        }
    }
}
