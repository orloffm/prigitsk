using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.Console;
using Prigitsk.Console.General;
using Prigitsk.Console.General.Programs;

namespace Prigitsk.Console.Verbs.Configure
{
    public sealed class ConfigureRunner : VerbRunnerBase<IConfigureRunnerOptions>, IConfigureRunner
    {
        private const string CleanSettingString = ".";
        private readonly IExternalAppPathProvider _appPathProvider;
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;

        public ConfigureRunner(
            IConfigureRunnerOptions options,
            IExternalAppPathProvider appPathProvider,
            IFileSystem fileSystem,
            IConsoleFactory consoleFactory,
            ILogger<ConfigureRunner> log) : base(
            options,
            log)
        {
            _appPathProvider = appPathProvider;
            _fileSystem = fileSystem;
            _console = consoleFactory.Create();
        }

        protected override void RunInternal()
        {
            bool allGood = true;
            foreach (ExternalApp p in _appPathProvider.EnumerateApps())
            {
                bool isOkNow = AskFor(p);
                allGood &= isOkNow;
            }

            if (allGood)
            {
                _console.WriteLine("All values set successfully.");
            }
            else
            {
                _console.WriteLine(
                    "Some of the values were not set successfully. Please rerun the configuration before using the application.");
            }
        }

        private bool AskFor(ExternalApp externalApp)
        {
            ExternalAppInfo info = _appPathProvider.GetAppInformation(externalApp);

            _console.WriteLine("=== Path to {0} application ({1}) ===", info.Title, info.ExeName);

            // Info about default app.
            _console.WriteLine("System app path: {0}", ToStringOr(info.FallbackPath, "not found"));

            // Info about app set in settings.
            _console.WriteLine("Current explicit path: {0}", ToStringOr(info.SettingsPath, "not set"));
            if (info.SettingsPathIsSet)
            {
                if (info.SettingsPathExists)
                {
                    _console.WriteLine("The specified file exists.");
                }
                else
                {
                    _console.WriteLine("The specified file does not exist.");
                }
            }

            bool appIsUsableAfterwards = false;
            bool enteredCorrectly = false;
            while (!enteredCorrectly)
            {
                enteredCorrectly = QueryAndUpdateSettingsValue(info, out appIsUsableAfterwards);
                if (!enteredCorrectly)
                {
                    _console.WriteLine("The entered path is not valid.");
                }
            }

            return appIsUsableAfterwards;
        }

        private bool ProcessEnteredValue(string enteredValue, ExternalAppInfo info, out bool appIsUsableAfterwards)
        {
            bool enteredCorrectly = true;

            if (string.IsNullOrWhiteSpace(enteredValue))
            {
                // User wants to use the current value.
                // So no changes to settings.

                // We're good if we have the fallback
                appIsUsableAfterwards = info.UsableApplicationIsPresent;
            }
            else if (enteredValue == CleanSettingString)
            {
                // Users wants to clean the set value.
                _appPathProvider.SetSettingsPathFor(info.App, string.Empty);

                // Can we use the system one?
                if (info.FallbackPathExists)
                {
                    appIsUsableAfterwards = true;
                    _console.WriteLine("The system app will be used.");
                }
                else
                {
                    appIsUsableAfterwards = false;
                }
            }
            else
            {
                string fullPath = null;
                try
                {
                    enteredValue = enteredValue.Trim(' ', '"');

                    fullPath = _fileSystem.Path.GetFullPath(enteredValue);
                    enteredCorrectly = _fileSystem.File.Exists(fullPath);
                }
                catch
                {
                    enteredCorrectly = false;
                }

                // Something specific was entered.
                if (enteredCorrectly)
                {
                    _appPathProvider.SetSettingsPathFor(info.App, fullPath);
                    _console.WriteLine($"{fullPath} will be used.");
                }

                appIsUsableAfterwards = enteredCorrectly;
            }

            return enteredCorrectly;
        }

        private bool QueryAndUpdateSettingsValue(ExternalAppInfo info, out bool appIsUsableAfterwards)
        {
            string enteredValue = WriteInvitationAndPerformQuery(info);

            bool enteredCorrectly = ProcessEnteredValue(enteredValue, info, out appIsUsableAfterwards);

            return enteredCorrectly;
        }

        private string ToStringOr(string currentPath, string fallbackValue)
        {
            bool shouldFallback = string.IsNullOrWhiteSpace(currentPath);
            if (shouldFallback)
            {
                return $"<{fallbackValue}>";
            }

            return currentPath;
        }

        private string WriteInvitationAndPerformQuery(ExternalAppInfo info)
        {
            // General invitation.
            _console.WriteLine($"Please enter the full path to {info.ExeName}.");
            if (info.FallbackPathExists)
            {
                _console.WriteLine($"(Enter {CleanSettingString} to use system app.)");
            }

            // We may omit this if there is something right now.
            if (info.UsableApplicationIsPresent)
            {
                _console.WriteLine("(Skip to keep the current setting.)");
            }

            _console.Write("> ");
            string enteredValue = _console.ReadLine();
            return enteredValue;
        }
    }
}