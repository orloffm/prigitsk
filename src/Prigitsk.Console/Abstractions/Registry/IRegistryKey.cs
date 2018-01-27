using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace Prigitsk.Console.Abstractions.Registry
{
    public interface IRegistryKey : IDisposable
    {
        void Close();

        object GetValue(string name);

        IRegistryKey OpenSubKey(string name);
    }
}