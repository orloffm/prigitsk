﻿namespace Prigitsk.Console.General.Programs
{
    public class ExternalAppInfo
    {
        public ExternalAppInfo(
            ExternalApp app,
            string title,
            string exeName,
            string settingsPath,
            bool settingsPathExists,
            string fallbackPath,
            bool fallbackPathExists)
        {
            App = app;
            Title = title;
            ExeName = exeName;

            SettingsPath = settingsPath;
            SettingsPathExists = settingsPathExists;

            FallbackPath = fallbackPath;
            FallbackPathExists = fallbackPathExists;
        }

        public ExternalApp App { get; }

        public string ExeName { get; }

        public string FallbackPath { get; set; }

        public bool FallbackPathExists { get; }

        public string SettingsPath { get; }

        public bool SettingsPathExists { get; }

        public bool SettingsPathIsSet => SettingsPathExists || !string.IsNullOrWhiteSpace(SettingsPath);

        public string Title { get; }

        public bool UsableApplicationIsPresent => SettingsPathExists || FallbackPathExists;
    }
}