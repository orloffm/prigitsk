namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunnerOptions : IDrawRunnerOptions
    {
        public DrawRunnerOptions(
            string repository,
            string target,
            string output,
            string format,
            string remoteToUse,
            bool forceTreatAsGitHub,
            bool leaveTails,
            bool removeTails,
            bool preventSimplification,
            bool keepAllOrphans,
            bool keepOrphansWithTags)
        {
            Repository = repository;
            TargetDirectory = target;
            OutputFileName = output;
            Format = format;
            RemoteToUse = remoteToUse;
            ForceTreatAsGitHub = forceTreatAsGitHub;
            LeaveHeads = leaveTails;
            RemoveTails = removeTails;
            PreventSimplification = preventSimplification;
            KeepAllOrphans = keepAllOrphans;
            KeepOrphansWithTags = keepOrphansWithTags;
        }

        public bool ForceTreatAsGitHub { get; }

        public string Format { get; }

        public bool KeepAllOrphans { get; }

        public bool KeepOrphansWithTags { get; }

        public bool LeaveHeads { get; }

        public string OutputFileName { get; }

        public bool PreventSimplification { get; }

        public string RemoteToUse { get; }

        public bool RemoveTails { get; }

        public string Repository { get; }

        public string TargetDirectory { get; }
    }
}