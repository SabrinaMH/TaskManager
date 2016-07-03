using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using NuGet;
using Squirrel;
using TaskManager.Domain.Infrastructure;
using ILogger = Serilog.ILogger;

namespace TaskManager
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            UpdateApp();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AppDomain.CurrentDomain.UnhandledException +=
                (sender, args) => HandleUnhandledException(args.ExceptionObject as Exception);
            Application.ThreadException +=
                (sender, args) => HandleUnhandledException(args.Exception);
            Application.Run(new MainForm());

        }

        private static void HandleUnhandledException(Exception exception)
        {
            ILogger logger = Logging.Logger;
            logger.Error(exception, "Unhandled exception caught in global error handler");
        }

        private static void UpdateApp()
        {
            SemanticVersion newVersion = null;
            var updated = false;

            var updateFolder = ConfigurationManager.AppSettings["installation.folder.update"];
            if (!Directory.Exists(updateFolder)) return;

            using (var mgr = new UpdateManager(updateFolder))
            {
                var updateInfo = mgr.CheckForUpdate().Result;

                var currentVersion = updateInfo.CurrentlyInstalledVersion.Version;
                Logging.Logger.Information("Checking for app updates");
                Logging.Logger.Information("Current version: {version}", currentVersion.ToString());

                if (updateInfo.ReleasesToApply.Any())
                {
                    newVersion = updateInfo.FutureReleaseEntry.Version;
                    Logging.Logger.Information("Found new version: {version}", newVersion.ToString());

                    mgr.DownloadReleases(updateInfo.ReleasesToApply).Wait();
                    Logging.Logger.Information("Downloaded new version: {version}", newVersion.ToString());

                    mgr.ApplyReleases(updateInfo).Wait();
                    Logging.Logger.Information("Applied new version: {version}", newVersion.ToString());
                    mgr.CreateUninstallerRegistryEntry().Wait();
                    Logging.Logger.Information("Created uninstaller for new version: {version}", newVersion.ToString());

                    updated = true;
                }
            }

            if (updated && newVersion != null)
            {
                Logging.Logger.Information("Restarting app");
                UpdateManager.RestartApp();
            }
        }
    }
}
