using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;
using Prigitsk.Console.Abstractions.Console;
using Prigitsk.Console.Abstractions.Settings;
using Prigitsk.Console.Tools;

namespace Prigitsk.Console.Verbs.Configure
{
    public class ConfigureRunner : VerbRunnerBase<IConfigureRunnerOptions>, IConfigureRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly ISettingsWrapper _settings;
        private readonly IConsole _console;
        private readonly IExeInformer _exeInformer;

        public ConfigureRunner(IConfigureRunnerOptions options, IFileSystem fileSystem, ISettingsWrapper settings, IConsoleFactory consoleFactory, IExeInformer exeInformer, ILogger log) : base(
            options,
            log)
        {
            _fileSystem = fileSystem;
            _settings = settings;
            _console = consoleFactory.Create();
            _exeInformer = exeInformer;
        }

        protected override void RunInternal()
        {
            AskForExe("Git", "git.exe", s => s.GitPath);
            AskForExe("GraphViz", "graphviz.exe", s => s.GraphVizPath);
        }

        private void AskForExe(string appTitle, string exeName, Expression<Func<ISettingsWrapper, string>> propertyExpression)
        {
            _console.WriteLine("Path to {0} application ({1}).", appTitle, exeName);

            MemberExpression memberExpression = propertyExpression.Body as MemberExpression;
            PropertyInfo propertyInfo = memberExpression.Member as PropertyInfo;
            string currentPath = propertyInfo.GetValue(_settings) as string;
            bool currentPathSet = !String.IsNullOrWhiteSpace(currentPath);
            _console.WriteLine("Current explicit path: {0}", ToStringOr(currentPath, "not set", currentPathSet));
            bool currentPathExists = currentPathSet && _fileSystem.File.Exists(currentPath);
            if (currentPathSet)
            {
                if (currentPathExists)
                {
                    _console.WriteLine("The specified file exists.");
                }
                else
                {
                    _console.WriteLine("The specified file does not exist.");
                }
            }

            string autoPath;
            bool autoPathExists = _exeInformer.TryFindFullPath(exeName, out autoPath);
            _console.WriteLine("Fallback path: {0}", ToStringOr(currentPath, "not found", autoPathExists));

            bool mustSet = !currentPathExists && !autoPathExists;
            string result = AskForValue(mustSet);

            //string value;
            //argsDic.TryGetValue(key, out value);
            //if (String.IsNullOrEmpty(value))
            //{
            //    value = propertyInfo.GetValue(settings) as string;
            //}
            //else
            //{
            //    propertyInfo.SetValue(settings, value);
            //}

            //if (String.IsNullOrEmpty(value))
            //{
            //    log.ErrorFormat("No {0} known. Please specify it as /{0}=\"abc\" in the command line.", key);
            //    return false;
            //}
            //else
            //{
            //    log.DebugFormat("Using {0}={1}.", key, value);
            //    return true;
            //}
        }
         
        private string AskForValue(bool mustSet)
        {
            throw new NotImplementedException();
        }

        private string ToStringOr(string currentPath, string fallbackValue, bool? explicitlyConfirm = null)
        {
            bool shouldFallback = (explicitlyConfirm == false) || string.IsNullOrWhiteSpace(currentPath);
            if (shouldFallback)
            {
                return $"<{fallbackValue}>";
            }

            return currentPath;
        }
    }
}