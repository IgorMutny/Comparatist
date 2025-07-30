namespace Comparatist.Services.TableCache
{
    internal class TableCache
    {
        public readonly Dictionary<Guid, CachedBlock> Blocks = new();
        public readonly Dictionary<Guid, CachedRow> Rows = new();
    }
}
