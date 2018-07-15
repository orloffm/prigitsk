using Prigitsk.Core.Graph;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Console.Verbs.Draw
{
    public sealed class DrawRunnerOptions : IDrawRunnerOptions
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
            bool includeOrphanedTags,
            bool noTags,
            int tagCount,
            string lesserBranchesRegex)
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
            IncludeOrphanedTags = includeOrphanedTags;
            TagCount = tagCount;
            LesserBranchesRegex = new LesserBranchRegex(lesserBranchesRegex);

            TagPickingMode = FigureOutTagPickingMode(noTags, tagCount);
        }

        public bool ForceTreatAsGitHub { get; }

        public string Format { get; }

        public bool IncludeOrphanedTags { get; }

        public bool KeepAllOrphans { get; }

        public bool LeaveHeads { get; }

        public string OutputFileName { get; }

        public bool PreventSimplification { get; }

        public string RemoteToUse { get; }

        public bool RemoveTails { get; }

        public string Repository { get; }

        public int TagCount { get; set; }

        public TagPickingMode TagPickingMode { get; }

        public string TargetDirectory { get; }

        public ILesserBranchRegex LesserBranchesRegex { get; }

        private TagPickingMode FigureOutTagPickingMode(bool noTags, int tagCount)
        {
            bool pickNone = noTags || tagCount == 0;
            if (pickNone)
            {
                return TagPickingMode.None;
            }

            bool pickAll = tagCount < 0;
            if (pickAll)
            {
                return TagPickingMode.All;
            }

            return TagPickingMode.Latest;
        }
    }
}