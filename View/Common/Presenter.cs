using Comparatist.Application.Management;

namespace Comparatist.View.Common
{
    internal abstract class Presenter<TRenderer, TInputHandler> : IPresenter
        where TRenderer : IRenderer
        where TInputHandler : IInputHandler
    {
        protected IProjectService Service { get; }
        protected TRenderer Renderer { get; }
        protected TInputHandler InputHandler { get; }

        public Presenter(IProjectService service, TRenderer renderer, TInputHandler inputHandler)
        {
            Service = service;
            Renderer = renderer;
            InputHandler = inputHandler;
            Subscribe();
        }

		public bool IsActive { get; protected set; }

		public void Dispose()
        {
            Unsubscribe();
            InputHandler?.Dispose();
            Renderer?.Dispose();
        }

        public void Show()
        {
            OnShow();
            Renderer.Show();
        }

        public void Hide()
        {
            Renderer.Hide();
            OnHide();
        }

        public abstract void RedrawAll();

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
        protected virtual void OnShow() { }
        protected virtual void OnHide() { }

        protected void Execute(Func<Result> func)
        {
            var result = func();
            if (!result.IsSuccess)
                Renderer.ShowError(result.Message);
        }

        protected TValue? Execute<TValue>(Func<Result<TValue>> func)
        {
            var result = func();

            if (result.IsSuccess && result.Value != null)
            {
                return result.Value;
            }
            else
            {
                Renderer.ShowError(result.Message);
                return default;
            }
        }
    }
}
