using Comparatist.Core.Records;

namespace Comparatist.Services.Cache
{
    public class CachedRoot
    {
        public required Root Record;
        public List<CachedStem> Stems = new();

        public object Clone()
        {
            return new CachedRoot
            {
                Record = (Root)Record.Clone(),
                Stems = Stems.Select(x => (CachedStem)x.Clone()).ToList()
            };
        }
    }
}
