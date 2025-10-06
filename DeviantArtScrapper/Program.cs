using DeviantArtScrapper.Forms;
using System.Globalization;
using System.Runtime.Versioning;

namespace DeviantArtScrapper;

/// <summary>
/// The main entry point class for the DeviantArt Scrapper application.
/// </summary>
[SupportedOSPlatform("windows")]
internal static class Program
{
    private const string MutexName = "DeviantArtScrapper_SingleInstance_Mutex";

    /// <summary>
    /// The main entry point for the application.
    /// Initializes the application and displays the main form.
    /// Ensures only one instance of the application can run at a time.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // Create a named mutex to ensure single instance
        using var mutex = new Mutex(true, MutexName, out bool createdNew);

        if (!createdNew)
        {
            // Another instance is already running
            MessageBox.Show(
                "DeviantArt Scrapper is already running.\n\n" +
                "Only one instance can run at a time to ensure proper API rate limiting " +
                "and prevent conflicts during scraping operations.",
                "Application Already Running",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
            return;
        }

        try
        {
            // Run the application
            Application.Run(new MainForm());
        }
        finally
        {
            // Mutex is automatically released when using statement ends
        }
    }
}
