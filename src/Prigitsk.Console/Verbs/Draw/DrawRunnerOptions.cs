using Prigitsk.Core.Graph;

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
            bool keepOrphansWithTags,
            bool noTags,
            int tagCount)
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
            TagCount = tagCount;

            SetTagPickingMode(noTags);
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

        public int TagCount { get; set; }

        public TagPickingMode TagPickingMode { get; set; }

        public string TargetDirectory { get; }

        private void SetTagPickingMode(bool noTags)
        {
            bool pickNone = noTags || TagCount == 0;
            if (pickNone)
            {
                TagPickingMode = TagPickingMode.None;
                return;
            }

            bool pickAll = TagCount < 0;
            if (pickAll)
            {
                TagPickingMode = TagPickingMode.All;
                return;
            }

            TagPickingMode = TagPickingMode.Latest;
        }
    }
}