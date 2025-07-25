namespace Comparatist
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());

            var test = new SaveTest();
        }
    }
}