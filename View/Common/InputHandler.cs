namespace Comparatist.View.Common
{
    internal abstract class InputHandler<T>: IInputHandler where T : Control
    {
        protected T Control { get; private set; }

        public InputHandler(T control)
        {
            Control = control;
        }

        public abstract void Dispose();
    }
}
