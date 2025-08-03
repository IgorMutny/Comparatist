using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedStem
    {
        public required Stem Record;
        public Dictionary<Guid, CachedWord?> Words = new();

        public object Clone()
        {
            return new CachedStem
            {
                Record = (Stem)Record.Clone(),
                Words = Words
                    .Select(pair => 
                        new KeyValuePair<Guid, CachedWord?>(
                            pair.Key,
                            pair.Value == null ? null : (CachedWord)pair.Value.Clone()))
                            .ToDictionary()
            };
        }
    }
}
