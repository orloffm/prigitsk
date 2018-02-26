using System.Diagnostics;

namespace Prigitsk.Console.Abstractions.Registry
{
    public sealed class RegistryKey : IRegistryKey
    {
        private readonly Microsoft.Win32.RegistryKey _inner;

        private RegistryKey(Microsoft.Win32.RegistryKey inner)
        {
            Debug.Assert(inner != null, "The inner registry key should never be null.");
            _inner = inner;
        }

        public void Close()
        {
            _inner.Close();
        }

        public void Dispose()
        {
            _inner.Dispose();
        }

        public object GetValue(string name)
        {
            return _inner.GetValue(name);
        }

        public IRegistryKey OpenSubKey(string name)
        {
            return Wrap(_inner.OpenSubKey(name));
        }

        internal static IRegistryKey Wrap(Microsoft.Win32.RegistryKey inner)
        {
            return inner == null ? null : new RegistryKey(inner);
        }
    }
}