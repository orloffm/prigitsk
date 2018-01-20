using System;
using Microsoft.Extensions.Logging;

namespace Prigitsk
{
    public static class LoggerSimpleExtensions
    {
        public static void Info(this ILogger logger, string message, params object[] args)
        {
            logger.LogInformation(message, args);
        }

        public static void Debug(this ILogger logger, string message, params object[] args)
        {
            logger.LogDebug(message, args);
        }

        public static void Trace(this ILogger logger, string message, params object[] args)
        {
            logger.LogTrace(message, args);
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

        public static void Fatal(this ILogger logger, string message, params object[] args)
        {
            logger.LogCritical(message, args);
        }

        public static void Warn(this ILogger logger, string message, params object[] args)
        {
            logger.LogWarning(message, args);
        }
    }
}