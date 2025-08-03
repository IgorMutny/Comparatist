using Comparatist.Core.Records;

namespace Comparatist.Services.TableCache
{
    public class CachedSection
    {
        public required Category Category;
        public List<CachedSection> Children = new();
        public List<CachedBlock> Blocks = new();
    }
}
