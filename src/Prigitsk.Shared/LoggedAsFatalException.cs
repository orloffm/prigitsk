using System;

namespace Prigitsk
{
    /// <summary>
    ///     An exception that indicates that the error was explicitly logged as a fatal one before being thrown.
    /// </summary>
    public class LoggedAsFatalException : Exception
    {
        public LoggedAsFatalException(string message) : base(message)
        {
        }

        public LoggedAsFatalException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}