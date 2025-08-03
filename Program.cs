namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Test.Run(10, 50, 1000, 10000);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}