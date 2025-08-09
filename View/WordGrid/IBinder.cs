using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal interface IBinder
    {
        Guid Id { get; }
        void Update(ICachedRecord cached);
    }
}
