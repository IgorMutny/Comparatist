using Comparatist.Services.Cache;

namespace Comparatist.View.WordGrid
{
    internal abstract class Binder<TCached>: IBinder
        where TCached : class, ICachedRecord 
    {
        protected Binder(TCached state, WordGridRenderer renderer)
        {
            CurrentState = state;
            PreviousState = state;
            Renderer = renderer;
        }
        
        public abstract Guid Id { get; }
        protected TCached PreviousState { get; private set; }
        protected TCached CurrentState { get; private set; }
        protected WordGridRenderer Renderer { get; private set; }

        public void Update(ICachedRecord cached)
        {
            if (cached is TCached typedCached)
                Update(typedCached);
        }

        private void Update(TCached newState)
        {
            CurrentState = newState;
            OnUpdate();
            PreviousState = newState;
        }

        protected virtual void OnCreate() { }
        protected abstract void OnUpdate();
    }
}
