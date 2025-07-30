using Comparatist.Services.Cache;

namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Test.Run(10, 200, 10000, 100000);
            ApplicationConfiguration.Initialize();
            var dataCacheService = new DataCacheService();
            Application.Run(new MainForm(dataCacheService));
        }
    }
}