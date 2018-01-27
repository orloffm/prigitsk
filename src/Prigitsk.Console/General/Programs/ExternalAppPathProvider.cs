using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.Settings;
using Prigitsk.Console.Tools;

namespace Prigitsk.Console.General.Programs
{
    public class ExternalAppPathProvider : IExternalAppPathProvider
    {
        private static readonly Dictionary<ExternalApp, string> Titles;
        private static readonly Dictionary<ExternalApp, string> ExeNames;
        private static readonly Dictionary<ExternalApp, Expression<Func<ISettingsWrapper, string>>> SettingsProps;
        private readonly IExeInformer _exeInformer;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _log;
        private readonly ISettingsWrapper _settings;

        static ExternalAppPathProvider()
        {
            Titles = new Dictionary<ExternalApp, string>
            {
                {ExternalApp.Git, "Git"},
                {ExternalApp.GraphViz, "GraphViz"}
            };
            ExeNames = new Dictionary<ExternalApp, string>
            {
                {ExternalApp.Git, "git.exe"},
                {ExternalApp.GraphViz, "dot.exe"}
            };
            SettingsProps = new Dictionary<ExternalApp, Expression<Func<ISettingsWrapper, string>>>
            {
                {ExternalApp.Git, w => w.GitPath},
                {ExternalApp.GraphViz, w => w.GraphVizPath}
            };
        }

        public ExternalAppPathProvider(
            IFileSystem fileSystem,
            IExeInformer exeInformer,
            ISettingsWrapper settings,
            ILogger log)
        {
            _fileSystem = fileSystem;
            _exeInformer = exeInformer;
            _settings = settings;
            _log = log;
        }

        public string GetProperAppPath(ExternalApp app)
        {
            string settingsPath = GetFullSettingsPathFor(app);
            if (_fileSystem.File.Exists(settingsPath))
            {
                return settingsPath;
            }

            string fallbackPath;
            string exeName = ExeNames[app];
            bool fallbackPathExists = _exeInformer.TryFindFullPath(exeName, out fallbackPath);
            if (fallbackPathExists)
            {
                return fallbackPath;
            }

            // Fail.
            string message =
                $"Cannot find path for {Titles[app]}. Please configure application by running it with {VerbConstants.Configure} option.";
            throw new Exception(message);
        }

        public IEnumerable<ExternalApp> EnumerateApps()
        {
            return Enum.GetValues(typeof(ExternalApp)).Cast<ExternalApp>();
        }

        public ExternalAppInfo GetAppInformation(ExternalApp app)
        {
            string settingsPath = GetFullSettingsPathFor(app);
            bool settingsPathExists = _fileSystem.File.Exists(settingsPath);
            string fallbackPath;
            string exeName = ExeNames[app];
            bool fallbackPathExists = _exeInformer.TryFindFullPath(exeName, out fallbackPath);

            return new ExternalAppInfo(
                app,
                Titles[app],
                exeName,
                settingsPath,
                settingsPathExists,
                fallbackPath,
                fallbackPathExists);
        }

        public string GetFullSettingsPathFor(ExternalApp app)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(app);

            string data = propertyInfo.GetValue(_settings) as string;
            if (string.IsNullOrWhiteSpace(data))
            {
                return null;
            }

            string fullPath = _fileSystem.Path.GetFullPath(data);
            return fullPath;
        }

        public void SetSettingsPathFor(ExternalApp app, string fullPath)
        {
            PropertyInfo propertyInfo = GetPropertyInfo(app);

            propertyInfo.SetValue(_settings, fullPath);
            _settings.Save();
        }

        private static PropertyInfo GetPropertyInfo(ExternalApp app)
        {
            MemberExpression memberExpression = (MemberExpression) SettingsProps[app].Body;
            PropertyInfo propertyInfo = (PropertyInfo) memberExpression.Member;
            return propertyInfo;
        }
    }
}