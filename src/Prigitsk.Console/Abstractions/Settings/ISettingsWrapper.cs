namespace Prigitsk.Console.Abstractions.Settings
{
    /// <summary>
    ///     A wrapper over the settings file.
    /// </summary>
    public interface ISettingsWrapper
    {
        /// <summary>
        ///     Path to GraphViz executable.
        /// </summary>
        string GraphVizPath { get; set; }

        /// <summary>
        ///     Persists changes.
        /// </summary>
        void Save();
    }
}