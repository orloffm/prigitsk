using System;

namespace Prigitsk.Core.Tools
{
    /// <summary>
    ///     Abstracts a general text writer.
    /// </summary>
    public interface ITextWriter : IDisposable
    {
        void Write(string value);

        void WriteLine(string line);

        void WriteLine(string format, params object[] arg);
    }
}