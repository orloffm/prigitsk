using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace Prigitsk.Console.Abstractions.Registry
{
    public interface IRegistry
    {
        IRegistryKey LocalMachine { get; }
    }
}