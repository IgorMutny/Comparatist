namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            //Test.Run(10, 2, 5000, 0);
            ApplicationConfiguration.Initialize();
            System.Windows.Forms.Application.Run(new MainForm());
        }
    }
}