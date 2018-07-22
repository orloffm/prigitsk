using System;
using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Tests.Graph
{
    public sealed class TagPickerTestCase
    {
        public TagPickerTestCase()
        {
            TagAName = "A";
            TagBName = "B";
            TagCName = "C";
            IncludeOrphanedTags = true;
        }

        public string BranchAName { get; set; }

        public string BranchBName { get; set; }

        public string BranchCName { get; set; }

        public DateTimeOffset? CommitADate { get; set; }

        public DateTimeOffset? CommitBDate { get; set; }

        public DateTimeOffset? CommitCDate { get; set; }

        public bool ExpectedTagAPicked { get; set; }

        public bool ExpectedTagBPicked { get; set; }

        public bool ExpectedTagCPicked { get; set; }

        public bool IncludeOrphanedTags { get; set; }

        public TagPickingMode Mode { get; set; }

        public string TagAName { get; set; }

        public string TagBName { get; set; }

        public string TagCName { get; set; }

        public int TakeCount { get; set; }
    }
}