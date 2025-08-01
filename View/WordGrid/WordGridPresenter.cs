using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.WordGrid
{
    internal class WordGridPresenter : Presenter<WordGridViewAdapter>
    {
        public WordGridPresenter(IProjectService service, WordGridViewAdapter view) :
            base(service, view)
        { }

        protected override void Subscribe()
        {
            throw new NotImplementedException();
        }

        protected override void Unsubscribe()
        {
            throw new NotImplementedException();
        }
    }
}
