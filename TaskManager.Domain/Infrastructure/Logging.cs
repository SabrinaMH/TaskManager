using System;
using System.IO;
using Serilog;
using Serilog.Debugging;
using Serilog.Exceptions;

namespace TaskManager.Domain.Infrastructure
{
    public class Logging
    {
        private static ILogger _logger;

        public static ILogger Logger
        {
            get
            {
                if (_logger == null)
                {
                    Bootstrap();
                }
                return _logger;
            }
        }

        private static void Bootstrap()
        {
            var logfile = "log-{Date}.txt";
            var rollingFile = Path.Combine(Environment.CurrentDirectory, logfile);
            var logger = new LoggerConfiguration()
                .ReadFrom.AppSettings()
                .WriteTo.RollingFile(rollingFile, retainedFileCountLimit: 7, fileSizeLimitBytes: 104857600)
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .CreateLogger();
            var file = File.CreateText(Path.Combine(Environment.CurrentDirectory, "serilog_error.txt"));
            SelfLog.Out = TextWriter.Synchronized(file);
            _logger = logger;
        }
    }
}