
namespace Prigitsk.Console.Abstractions.Registry
{
    public class Registry : IRegistry
    {
        public IRegistryKey LocalMachine
        {
            get { return RegistryKey.Wrap(Microsoft.Win32.Registry.LocalMachine); }
        }
    }
}
