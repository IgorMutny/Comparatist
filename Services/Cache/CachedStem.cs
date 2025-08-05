using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedStem
    {
        public required Stem Record { get; set; }
        public Dictionary<Guid, CachedWord> WordsByLanguage { get; set; } = new();

        public object Clone()
        {
            return new CachedStem
            {
                Record = (Stem)Record.Clone(),
                WordsByLanguage = WordsByLanguage
                    .Select(pair => 
                        new KeyValuePair<Guid, CachedWord>(
                            pair.Key,
                            (CachedWord)pair.Value.Clone()))
                            .ToDictionary()
            };
        }
    }
}
