using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedRoot
    {
        public required Root Record;
        public Dictionary<Guid, CachedStem> Stems = new();

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
    }
}
