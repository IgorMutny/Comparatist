using Comparatist.Services.Infrastructure;

namespace Comparatist.View.Infrastructure
{
    internal abstract class Presenter<T> : IDisposable where T : ViewAdapter
    {
        protected IProjectService Service { get; }
        protected T View { get; }

        public Presenter(IProjectService service, T view)
        {
            Service = service;
            View = view;
            Subscribe();
        }

        public void Dispose()
        {
            Unsubscribe();
        }

        protected abstract void Subscribe();
        protected abstract void Unsubscribe();

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
                return default(TValue);
            }
        }
    }
}
