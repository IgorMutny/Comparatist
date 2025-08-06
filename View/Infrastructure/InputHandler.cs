namespace Comparatist.View.Infrastructure
{
    internal abstract class InputHandler<T>: IDisposable where T : Control
    {
        protected T Control { get; private set; }

        public InputHandler(T control)
        {
            Control = control;
        }

        public abstract void Dispose();
    }
}
