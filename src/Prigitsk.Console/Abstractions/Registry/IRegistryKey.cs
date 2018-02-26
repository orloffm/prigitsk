using System;

namespace Prigitsk.Console.Abstractions.Registry
{
    public interface IRegistryKey : IDisposable
    {
        void Close();

        object GetValue(string name);

        IRegistryKey OpenSubKey(string name);
    }
}