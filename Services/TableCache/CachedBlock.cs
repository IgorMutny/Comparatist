using Comparatist.Core.Records;

namespace Comparatist.Services.TableCache
{
    public class CachedBlock : ICloneable
    {
        public required Root Root;
        public Dictionary<Guid, CachedRow> Rows = new();

        public object Clone()
        {
            return new CachedBlock
            {
                Root = (Root)Root.Clone(),
                Rows = Rows
                    .Select(
                        pair => new KeyValuePair<Guid, CachedRow>(
                            pair.Key,
                            (CachedRow)pair.Value.Clone()))
                    .ToDictionary()
            };
        }
    }
}
