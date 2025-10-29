using NLog;
using System.Diagnostics;

namespace TodoTxt.Core.CrossCutting.Logging;

public static class LogConfigurer
{

    public static void ConfigureAppFaultLogging()
    {
        ConfigureLogging(LogLevel.Debug, LogLevel.Fatal, true, true, true, DefaultLogFile);
    }
    public static void ConfigureDefaultLogging(bool isDebug, string logFileName = null)
    {
        var debugger = Debugger.IsAttached;
        ConfigureLogging(isDebug ? LogLevel.Debug : LogLevel.Info, LogLevel.Fatal, isDebug || debugger, debugger, true, logFileName);
    }

    public static void ConfigureLogging(LogLevel minLogLevel, LogLevel maxLogLevel, bool debugTarget, bool consoleTarget, bool fileTarget, string logFileName = null)
    {
        var config = new NLog.Config.LoggingConfiguration();
        var debug = new NLog.Targets.DebugTarget("debug") {
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}",
        };
        var console = new NLog.Targets.ConsoleTarget("console") {
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}",
        };
        var logfile = new NLog.Targets.FileTarget("logfile") {
            FileName = !string.IsNullOrEmpty(logFileName) ? logFileName : DefaultLogFile,
            Layout = "${longdate} ${level:uppercase=true} ${message} ${exception:format=tostring}",
            ArchiveAboveSize = 1024 * 1024 * 10,
            MaxArchiveFiles = 5,
        };

        if (debugTarget) {
            config.AddRule(minLogLevel, maxLogLevel, debug);
        }
        if (consoleTarget) {
            config.AddRule(minLogLevel, maxLogLevel, console);
        }
        if (fileTarget) {
            config.AddRule(minLogLevel, maxLogLevel, logfile);
        }
        LogManager.Configuration = config;
        LogManager.ReconfigExistingLoggers();
    }



    private static Lazy<string> _defaultLogFileName = new Lazy<string>(() => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "todotxt.desktop", "log.txt"));

    public static string DefaultLogFile => _defaultLogFileName.Value;

}