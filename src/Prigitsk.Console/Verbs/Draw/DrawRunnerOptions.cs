using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Graph;

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
            bool keepOrphansWithTags,
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
            KeepOrphansWithTags = keepOrphansWithTags;
            TagCount = tagCount;
            LesserBranchesRegex = lesserBranchesRegex;

            TagPickingMode = FigureOutTagPickingMode(noTags);
        }

        public bool ForceTreatAsGitHub { get; }

        public string Format { get; }

        public bool KeepAllOrphans { get; }

        public bool KeepOrphansWithTags { get; }

        public bool LeaveHeads { get; }

        public string LesserBranchesRegex { get; }

        public string OutputFileName { get; }

        public bool PreventSimplification { get; }

        public string RemoteToUse { get; }

        public bool RemoveTails { get; }

        public string Repository { get; }

        public int TagCount { get; set; }

        public TagPickingMode TagPickingMode { get; }

        public string TargetDirectory { get; }

        private TagPickingMode FigureOutTagPickingMode(bool noTags)
        {
            bool pickNone = noTags || TagCount == 0;
            if (pickNone)
            {
                return TagPickingMode.None;
            }

            bool pickAll = TagCount < 0;
            if (pickAll)
            {
                return TagPickingMode.All;
            }

            return TagPickingMode.Latest;
        }
    }
}