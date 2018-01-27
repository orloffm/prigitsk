namespace Prigitsk.Console.Abstractions.Registry
{
    public interface IRegistry
    {
        IRegistryKey LocalMachine { get; }
    }
}