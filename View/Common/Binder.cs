using Comparatist.Application.Cache;
using Comparatist.View.WordGrid;

namespace Comparatist.View.Common
{
    internal abstract class Binder<TCached, TRenderer>: IBinder
        where TCached : class, ICachedRecord
        where TRenderer : class, IRenderer
    {
        protected Binder(TCached state, TRenderer renderer)
        {
            CurrentState = state;
            PreviousState = (TCached)state.Clone();
            Renderer = renderer;
        }
        
        public abstract Guid Id { get; }
        public TCached CurrentState { get; private set; }
        protected TCached PreviousState { get; private set; }
        protected TRenderer Renderer { get; private set; }

        public void Update(ICachedRecord cached)
        {
            if (cached is TCached typedCached)
                Update(typedCached);
        }

        private void Update(TCached newState)
        {
            CurrentState = newState;
            OnUpdate();
            PreviousState = (TCached)newState.Clone();
        }

        public virtual void OnCreate() { }
        protected abstract void OnUpdate();
    }
}
