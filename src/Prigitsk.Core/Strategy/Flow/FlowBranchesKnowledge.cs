using System.Collections.Generic;
using System.Drawing;
using Prigitsk.Framework;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchesKnowledge : BranchesKnowledgeBase
    {
        private readonly Dictionary<BranchLogicalType, Color> _typesToColors;
        private readonly IMultipleDictionary<BranchLogicalType, string> _typesToRegices;

        public FlowBranchesKnowledge(
            ILesserBranchRegex workItemRegex,
            IWorkItemBranchSelectorFactory workItemBranchSelectorFactory
        ) : base(workItemRegex, workItemBranchSelectorFactory)
        {
            _typesToColors = new Dictionary<BranchLogicalType, Color>
            {
                // Master goes first, because we need to see "these guys are on PROD".
                {BranchLogicalType.Master, Color.FromArgb(82, 0, 198)},
                // Hotfix are on top of master.
                {BranchLogicalType.MasterHotfix, Color.FromArgb(253, 89, 101)},
                // What will get in the next release?
                {BranchLogicalType.Release, Color.FromArgb(82, 195, 34)},
                // What is integrated and planned for the next release?
                {BranchLogicalType.Integration, Color.FromArgb(127, 171, 232)},
                // What is in develop that is not in any other branches?
                {BranchLogicalType.Develop, Color.FromArgb(234, 175, 0)},
                // What are the features developed on top of develop?
                {BranchLogicalType.DevelopFeature, Color.FromArgb(186, 83, 27)},
                // What are the work items developed on top of develop.
                {BranchLogicalType.WorkItem, Color.FromArgb(201, 130, 175)}
            };

            _typesToRegices = new MultipleDictionary<BranchLogicalType, string>
            {
                {BranchLogicalType.Master, new[] {"^master$"}},
                {BranchLogicalType.Develop, new[] {"^develop$"}},
                {BranchLogicalType.DevelopFeature, new[] {"^dev", "-dev-"}},
                {BranchLogicalType.Release, new[] {"^release", "-RC$"}},
                {BranchLogicalType.MasterHotfix, new[] {"hotfix", "^master"}},
                {BranchLogicalType.Integration, new[] {"^int"}}
            };
        }

        protected override Color GetColorInternal(BranchLogicalType ft)
        {
            return _typesToColors[ft];
        }

        protected override bool TryGetRegexStringsInternal(BranchLogicalType flowType, out ISet<string> regexStrings)
        {
            return _typesToRegices.TryGetValue(flowType, out regexStrings);
        }
    }
}