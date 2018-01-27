using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prigitsk.Console.Abstractions.Console
{
    public interface IConsole
    {
        void WriteLine();
        void WriteLine(string format, params object[] arg);
        void WriteLine(string text);
    }
}
