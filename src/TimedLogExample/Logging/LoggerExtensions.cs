namespace TimedLogExample.Logging;

public static class LoggerExtensions
{
    public static IDisposable TimedOperation<T>(this ILogger<T> logger, string message, params object?[] args)
        => new TimedLogOperation<T>(logger, LogLevel.Information, message, args);
}