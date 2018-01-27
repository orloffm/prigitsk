using System.Collections.Generic;

namespace Prigitsk.Console.General.Programs
{
    public interface IExternalAppPathProvider
    {
        /// <summary>
        ///     Gets the full path to the external program. The executable exists, or an exception is thrown.
        /// </summary>
        string GetProperAppPath(ExternalApp app);

        /// <summary>
        ///     Lists all programs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ExternalApp> EnumerateApps();

        ExternalAppInfo GetAppInformation(ExternalApp app);

        void SetSettingsPathFor(ExternalApp app, string fullPath);

        string GetFullSettingsPathFor(ExternalApp app);
    }
}