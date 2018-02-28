namespace Prigitsk.Console.Abstractions.Registry
{
    public class Registry : IRegistry
    {
        public IRegistryKey LocalMachine => RegistryKey.Wrap(Microsoft.Win32.Registry.LocalMachine);
    }
}