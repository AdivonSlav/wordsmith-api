using System.Reflection;
using System.Runtime.CompilerServices;
using NLog;

namespace Wordsmith.Utils;

public static class Logger
{
    public static bool Enabled
    {
        get => LogManager.IsLoggingEnabled();
        set
        {
            if (value)
            {
                LogManager.ResumeLogging();
            }
            else
            {
                LogManager.SuspendLogging();
            }
        }
    }

    public static void Init()
    {
        _ = LogManager.Setup().LoadConfigurationFromAssemblyResource(typeof(Logger).GetTypeInfo().Assembly);
        LogDebug("NLog initialized with the XML config in the Utils library");
    }

    public static void Cleanup()
    {
        LogManager.Shutdown();
    }
    
    public static void LogDebug(
        string message,
        Exception? exception = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Debug, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine);
    }
    
    public static void LogInfo(
        string message,
        Exception? exception = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Info, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine);
    }
    
    public static void LogWarn(
        string message,
        Exception? exception = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Warn, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine);
    }
    
    public static void LogError(
        string message,
        Exception? exception = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Error, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine);
    }
    
    public static void LogFatal(
        string message,
        Exception? exception = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Fatal, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine);
    }
    
    private static void Log(
        LogLevel level,
        string message,
        string? assemblyFullName,
        Exception? exception = null,
        string callerPath = "",
        string callerMember = "",
        int callerLine = 0)
    {
        var logger = LogManager.GetLogger(callerPath);

        if (!logger.IsEnabled(level)) return;

        callerPath = FormatCallerPath(callerPath, assemblyFullName);
        
        var logEvent = new LogEventInfo(level, callerPath, message) { Exception = exception };
        logEvent.Properties.Add("callerpath", callerPath);
        logEvent.Properties.Add("callermember", callerMember);
        logEvent.Properties.Add("callerline", callerLine);
        logger.Log(logEvent);
    }

    private static string FormatCallerPath(string callerPath, string? assemblyFullName)
    {
        var tmp = callerPath.LastIndexOf($"src{Path.DirectorySeparatorChar}", StringComparison.Ordinal) + 4;
        callerPath = callerPath.Substring(tmp, callerPath.Length - tmp);
        var assemblyName = assemblyFullName?.Split(", ")[0];
        
        return $"{assemblyName}::{callerPath}";
    }
}