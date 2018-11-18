using System.Collections.Generic;
using System.Drawing;
using Prigitsk.Framework;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchesColorsAndRegices : IBranchesColorsAndRegices
    {
        private readonly Dictionary<BranchLogicalType, Color> _typesToColors;
        private readonly IMultipleDictionary<BranchLogicalType, string> _typesToRegices;

        public FlowBranchesColorsAndRegices()
        {
            _typesToColors = new Dictionary<BranchLogicalType, Color>
            {
                {BranchLogicalType.Master, Color.FromArgb(82, 0, 198)}
                , {BranchLogicalType.MasterHotfix, Color.FromArgb(253, 89, 101)}
                , {BranchLogicalType.Release, Color.FromArgb(82, 195, 34)}
                , {BranchLogicalType.Integration, Color.FromArgb(127, 171, 232)}
                , {BranchLogicalType.Develop, Color.FromArgb(234, 175, 0)}
                , {BranchLogicalType.DevelopFeature, Color.FromArgb(186, 83, 27)}
                , {BranchLogicalType.WorkItem, Color.FromArgb(201, 130, 175)}
            };

            _typesToRegices = new MultipleDictionary<BranchLogicalType, string>
            {
                {BranchLogicalType.Master, new[] {"^master$"}}, {BranchLogicalType.Develop, new[] {"^develop$"}}
                , {BranchLogicalType.DevelopFeature, new[] {"^dev", "-dev-"}}
                , {BranchLogicalType.Release, new[] {"^release", "-RC$"}}
                , {BranchLogicalType.MasterHotfix, new[] {"hotfix", "^master"}}
                , {BranchLogicalType.Integration, new[] {"^int"}}
            };
        }

        public BranchLogicalType[] GetAllBranchLogicalTypesOrdered()
        {
            return new[]
            {
                // Master goes first, because we need to see "this is on PROD".
                BranchLogicalType.Master
                // Hotfix are on top of master.
                , BranchLogicalType.MasterHotfix
                // What will get in the next release?
                , BranchLogicalType.Release
                // What is integrated and planned for the next release?
                , BranchLogicalType.Integration
                // What is in develop that is not in any other branches?
                , BranchLogicalType.Develop
                // What are the features developed on top of develop?
                , BranchLogicalType.DevelopFeature
                // What are the work items developed on top of develop.
                , BranchLogicalType.WorkItem
            };
        }

        public Color GetColor(BranchLogicalType ft)
        {
            return _typesToColors[ft];
        }

        public bool TryGetRegexStringsInternal(BranchLogicalType flowType, out ISet<string> regexStrings)
        {
            return _typesToRegices.TryGetValue(flowType, out regexStrings);
        }
    }
}