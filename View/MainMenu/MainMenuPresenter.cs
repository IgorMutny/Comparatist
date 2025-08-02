using Comparatist.Services.Infrastructure;
using Comparatist.View.Infrastructure;

namespace Comparatist.View.MainMenu
{
    internal class MainMenuPresenter : Presenter<MainMenuViewAdapter>
    {
        public MainMenuPresenter(IProjectService service, MainMenuViewAdapter view) :
            base(service, view)
        { }

        protected override void Subscribe()
        {
            View.LoadRequest += Load;
            View.SaveRequest += Save;
        }

        protected override void Unsubscribe()
        {
            View.LoadRequest -= Load;
            View.SaveRequest -= Save;
        }

        protected override void UpdateView() { }

        private void Load(string filePath)
        {
            Execute(() => Service.LoadDatabase(filePath));
        }

        private void Save(string filePath)
        {
            Execute(() => Service.SaveDatabase(filePath));
        }
    }
}
