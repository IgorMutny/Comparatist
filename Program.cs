using Comparatist.Services.TableCache;

namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Test.Run(10, 200, 10000, 100000);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}