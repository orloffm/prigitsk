namespace Prigitsk.Console.General.Programs
{
    public interface IExternalAppPathProvider
    {
        /// <summary>
        ///     Lists all programs.
        /// </summary>
        /// <returns></returns>
        IEnumerable<ExternalApp> EnumerateApps();

        ExternalAppInfo GetAppInformation(ExternalApp app);

        string GetFullSettingsPathFor(ExternalApp app);

        /// <summary>
        ///     Gets the full path to the external program. The executable exists, or an exception is thrown.
        /// </summary>
        string GetProperAppPath(ExternalApp app);

        void SetSettingsPathFor(ExternalApp app, string fullPath);
    }
}