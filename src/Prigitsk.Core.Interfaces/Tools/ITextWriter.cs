using System;

namespace Prigitsk.Core.Tools
{
    /// <summary>
    ///     Abstracts a general text writer.
    /// </summary>
    public interface ITextWriter : IDisposable
    {
        void Append(string value);

        void AppendLine(string line);

        void AppendLine(string format, params object[] arg);
    }
}