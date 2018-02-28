using System;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.Registry;

namespace Prigitsk.Console.Tools
{
    public class ExeInformer : IExeInformer
    {
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _log;
        private readonly IRegistry _registry;

        public ExeInformer(IFileSystem fileSystem, IRegistry registry, ILogger log)
        {
            _fileSystem = fileSystem;
            _registry = registry;
            _log = log;
        }

        public bool TryFindFullPath(string exeName, out string fullPath)
        {
            // Exists locally?
            if (TryFindLocally(exeName, out fullPath))
            {
                return true;
            }

            // Exists in registry?
            if (TryFindInRegistry(exeName, out fullPath))
            {
                return true;
            }

            // Exists in PATH?
            if (TryFindInPathVariable(exeName, out fullPath))
            {
                return true;
            }

            fullPath = null;
            return false;
        }

        private bool TryFindInPathVariable(string exeName, out string fullPath)
        {
            string path = Environment.GetEnvironmentVariable("path");
            string[] folders = path.Split(';');
            foreach (string dir in folders)
            {
                string particularPath = _fileSystem.Path.Combine(dir, exeName);
                if (_fileSystem.File.Exists(particularPath))
                {
                    _log.Debug("Found {0} in {1} (from PATH).", exeName, dir);
                    fullPath = particularPath;
                    return true;
                }
            }

            fullPath = null;
            return false;
        }

        private bool TryFindInRegistry(string exeName, out string fullPath)
        {
            fullPath = null;

            const string keyBase = @"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths";
            string expectedKey = string.Format(@"{0}\{1}", keyBase, exeName);

            IRegistryKey hklm = _registry.LocalMachine;
            IRegistryKey fileKey = hklm.OpenSubKey(expectedKey);
            if (fileKey == null)
            {
                return false;
            }

            object result;
            try
            {
                result = fileKey.GetValue(string.Empty);
            }
            finally
            {
                fileKey.Close();
            }

            if (result == null)
            {
                return false;
            }

            string suggestedPath = result.ToString();
            bool exists = _fileSystem.File.Exists(suggestedPath);
            if (!exists)
            {
                _log.Debug(
                    "{0} was specified in registry to be located at {1}, but it wasn't found there.",
                    exeName,
                    suggestedPath);
                return false;
            }

            _log.Debug("Found {0} at {1} (via registry).", exeName, suggestedPath);
            fullPath = suggestedPath;
            return true;
        }

        private bool TryFindLocally(string exeName, out string fullPath)
        {
            string dir = _fileSystem.Directory.GetCurrentDirectory();
            string localPath = _fileSystem.Path.Combine(dir, exeName);
            bool exists = _fileSystem.File.Exists(localPath);

            if (!exists)
            {
                fullPath = null;
                return false;
            }

            _log.Debug("Found {0} locally in {1}.", exeName, dir);
            fullPath = localPath;
            return true;
        }
    }
}