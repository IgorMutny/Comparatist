namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Test.Run(10000, 0, 0, 0);
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}