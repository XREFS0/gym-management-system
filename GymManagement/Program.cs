namespace GymManagement;

static class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        ApplicationConfiguration.Initialize();

        if (args.Length > 0 && args[0] == "--capture-screenshots")
        {
            string outDir = args.Length > 1 ? args[1] : System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "screenshots");
            ScreenshotCaptureUtility.RunCapture(System.IO.Path.GetFullPath(outDir));
            return;
        }

        Application.Run(new LoginForm());
    }    
}