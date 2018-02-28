using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk
{
    public static class LoggerSimpleExtensions
    {
        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }

        public static void Error(this ILogger logger, string message, params object[] args)
        {
            logger.LogError(message, args);
        }

        public static void Error(this ILogger logger, Exception ex, string message, params object[] args)
        {
            logger.LogError(ex, message, args);
        }

        public static void Error(this ILogger logger, Exception ex)
        {
            logger.LogError(ex, "An exception has occured.");
        }

        public static void Fatal(this ILogger logger, Exception ex, string message, params object[] args)
        {
            logger.LogCritical(ex, message, args);
        }

        public static void Fatal(this ILogger logger, Exception ex)
        {
            logger.LogCritical(ex, "An exception has occured.");
        }

        public static void Fatal(this ILogger logger, string message, params object[] args)
        {
            logger.LogCritical(message, args);
        }

        public static void Info(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public static void Log(this ILogger logger, LogLevel? level, string message, params object[] args)
        {
            if (level == null)
            {
                return;
            }

            logger.Log(level.Value, 0, string.Format(message, args), null, (s, ex) => s);
        }

        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            logger.LogTrace(message, args);
        }

        public static void Warn(this ILogger logger, string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }
    }
}