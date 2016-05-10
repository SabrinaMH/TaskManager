using System;
using System.Windows.Forms;
using Serilog;
using TaskManager.Domain.Infrastructure;

namespace TaskManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
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
    }
}
