using System.Collections.Generic;
using System.Linq;
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
            string lesserBranchesRegex,
            IEnumerable<string> includeBranchesRegices,
            IEnumerable<string> excludeBranchesRegices)
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
            IncludeBranchesRegices = includeBranchesRegices?.ToArray();
            ExcludeBranchesRegices = excludeBranchesRegices?.ToArray();

            TagPickingMode = FigureOutTagPickingMode(noTags, tagCount);
        }

        public bool ForceTreatAsGitHub { get; }

        public string Format { get; }

        public string[] IncludeBranchesRegices { get; }

        public string[] ExcludeBranchesRegices { get; }

        public bool IncludeOrphanedTags { get; }

        public bool KeepAllOrphans { get; }

        public bool LeaveHeads { get; }

        public ILesserBranchRegex LesserBranchesRegex { get; }

        public string OutputFileName { get; }

        public bool PreventSimplification { get; }

        public string RemoteToUse { get; }

        public bool RemoveTails { get; }

        public string Repository { get; }

        public int TagCount { get; set; }

        public TagPickingMode TagPickingMode { get; }

        public string TargetDirectory { get; }

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