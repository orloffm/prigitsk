using Microsoft.Extensions.Logging;
using Prigitsk.Console.Properties;

namespace Prigitsk.Console.Abstractions.Settings
{
    internal class SettingsWrapper : ISettingsWrapper
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger _log;

        public SettingsWrapper(AppSettings appSettings, ILogger<SettingsWrapper> log)
        {
            _appSettings = appSettings;
            _log = log;
        }

        public string GraphVizPath
        {
            get => _appSettings.GraphVizPath;
            set => _appSettings.GraphVizPath = value;
        }

        public void Save()
        {
            _appSettings.Save();
            _log.Debug("Saved settings.");
        }
    }
}