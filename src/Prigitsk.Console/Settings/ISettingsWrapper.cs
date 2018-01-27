﻿namespace Prigitsk.Console.Settings
{
    public interface ISettingsWrapper
    {
        /// <summary>
        ///     Path to Git executable.
        /// </summary>
        string GitPath { get; set; }

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