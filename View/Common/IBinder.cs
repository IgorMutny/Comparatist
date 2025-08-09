using Comparatist.Application.Cache;

namespace Comparatist.View.Common
{
    internal interface IBinder
    {
        Guid Id { get; }
        void OnCreate();
        void Update(ICachedRecord cached);
    }
}
