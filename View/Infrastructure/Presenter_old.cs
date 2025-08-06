using Comparatist.Services.Infrastructure;

namespace Comparatist.View.Infrastructure
{
    internal abstract class Presenter_old<TAdapter> : IDisposable where TAdapter : IViewAdapter_old
    {
        protected IProjectService Service { get; }
        protected TAdapter View { get; }

        public Presenter_old(IProjectService service, TAdapter view)
        {
            Service = service;
            View = view;
            View.RenderRequest += UpdateView;
            Subscribe();
        }

        public void Dispose()
        {
            View.RenderRequest -= UpdateView;
            Unsubscribe();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();
        protected abstract void UpdateView();

        protected void Execute(Func<Result> func)
        {
            var result = func();
            if (!result.IsSuccess)
                View.ShowError(result.Message);
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
                View.ShowError(result.Message);
                return default;
            }
        }
    }
}
