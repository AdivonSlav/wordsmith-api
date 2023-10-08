using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
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

    /// <summary>
    /// Initializes the wrapper class for NLog and sets the passed log level
    /// </summary>
    /// <param name="minLogLevel">The minimum log level to use (e.g. Debug)</param>
    public static void Init(string minLogLevel)
    {
        _ = LogManager.Setup().LoadConfigurationFromAssemblyResource(typeof(Logger).GetTypeInfo().Assembly);
        LogManager.Configuration.Variables["minLogLevel"] = minLogLevel;
        LogManager.ReconfigExistingLoggers();
        
        LogDebug("Initialized NLog with the XML config in the Utils library");
    }

    /// <summary>
    /// Shuts down all logging and disposes any targets specified in NLog.config
    /// </summary>
    public static void Cleanup()
    {
        LogManager.Shutdown();
    }
    
    public static void LogDebug(
        string message,
        Exception? exception = null,
        object? additionalArg = null,
        [CallerFilePath] string callerPath = "",
        [CallerMemberName] string callerMember = "",
        [CallerLineNumber] int callerLine = 0)
    {
        Log(LogLevel.Debug, message, Assembly.GetCallingAssembly().FullName, exception, callerPath, callerMember, callerLine, additionalArg);
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

    /// <summary>
    /// Takes information on a log and routes it to NLog
    /// </summary>
    /// <param name="level">Level of the log (e.g. Debug)</param>
    /// <param name="message">Log message</param>
    /// <param name="assemblyFullName">Name of the assembly that called the log method</param>
    /// <param name="additionalArg">An additional argument</param>
    /// <param name="exception">An optional exception to be passed</param>
    /// <param name="callerPath"></param>
    /// <param name="callerMember"></param>
    /// <param name="callerLine"></param>
    private static void Log(
        LogLevel level,
        string message,
        string? assemblyFullName,
        Exception? exception = null,
        string callerPath = "",
        string callerMember = "",
        int callerLine = 0,
        object? additionalArg = null)
    {
        var logger = LogManager.GetLogger(callerPath);

        if (!logger.IsEnabled(level)) return;

        callerPath = FormatCallerPath(callerPath, assemblyFullName);
        
        var logEvent = new LogEventInfo(level, callerPath, message) { Exception = exception };
        
        if (additionalArg != null)
        {
            logEvent.Message += $" @additionalArg";
            logEvent.Parameters = new object[] { additionalArg };
        }
        
        logEvent.Properties.Add("caller_path", callerPath);
        logEvent.Properties.Add("caller_member", callerMember);
        logEvent.Properties.Add("caller_line", callerLine);
        logger.Log(logEvent);
    }

    /// <summary>
    /// Formats the [CallerFilePath] output based on the assembly name
    /// </summary>
    /// <param name="callerPath">Filepath to the class that called a method</param>
    /// <param name="assemblyFullName">Name of the assembly of the class</param>
    /// <returns>A formatted caller path output</returns>
    private static string FormatCallerPath(string callerPath, string? assemblyFullName)
    {
        var tmp = callerPath.LastIndexOf($"src{Path.DirectorySeparatorChar}", StringComparison.Ordinal) + 4;
        callerPath = callerPath.Substring(tmp, callerPath.Length - tmp);
        var assemblyName = assemblyFullName?.Split(", ")[0];
        
        return $"{assemblyName}::{callerPath}";
    }

    private static string SerializeArguments(IEnumerable<object> args)
    {
        var output = "";
        
        foreach (var argument in args)
        {
            var serialized = JsonSerializer.Serialize(argument);
            output += serialized + " ";
        }

        return output;
    }
}