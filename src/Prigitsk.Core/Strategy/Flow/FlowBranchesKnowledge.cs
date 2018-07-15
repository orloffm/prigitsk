using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Entities.Comparers;
using Prigitsk.Framework;

namespace Prigitsk.Core.Strategy.Flow
{
    public sealed class FlowBranchesKnowledge : IBranchesKnowledge
    {
        private readonly List<IBranch> _branchesInLogicalOrder;
        private readonly Dictionary<IBranch, FlowBranchLogicalType> _branchesToTypes;
        private readonly IWorkItemBranchSelector _lesserBranchSelector;
        private readonly FlowBranchingStrategy _strategy;

        private readonly Dictionary<FlowBranchLogicalType, Color> _typesToColors;
        private readonly IMultipleDictionary<FlowBranchLogicalType, string> _typesToRegices;
        private readonly ILesserBranchRegex _workItemRegex;

        public FlowBranchesKnowledge(
            FlowBranchingStrategy strategy,
            ILesserBranchRegex workItemRegex,
            IWorkItemBranchSelectorFactory workItemBranchSelectorFactory)
        {
            _strategy = strategy;
            _workItemRegex = workItemRegex;
            _lesserBranchSelector = workItemBranchSelectorFactory.MakeSelector();

            _branchesToTypes = new Dictionary<IBranch, FlowBranchLogicalType>();
            _branchesInLogicalOrder = new List<IBranch>();

            _typesToColors = new Dictionary<FlowBranchLogicalType, Color>
            {
                {FlowBranchLogicalType.Master, Color.FromArgb(39, 228, 249)},
                {FlowBranchLogicalType.Develop, Color.FromArgb(255, 227, 51)},
                {FlowBranchLogicalType.DevelopFeature, Color.FromArgb(251, 61, 181)},
                {FlowBranchLogicalType.Release, Color.FromArgb(82, 195, 34)},
                {FlowBranchLogicalType.MasterHotfix, Color.FromArgb(253, 89, 101)},
                {FlowBranchLogicalType.WorkItem, Color.FromArgb(201, 130, 175)}
            };

            _typesToRegices = new MultipleDictionary<FlowBranchLogicalType, string>
            {
                {FlowBranchLogicalType.Master, new[] {"^master$"}},
                {FlowBranchLogicalType.Develop, new[] {"^develop$"}},
                {FlowBranchLogicalType.DevelopFeature, new[] {"^dev", "-dev-"}},
                {FlowBranchLogicalType.Release, new[] {"^release", "-RC$"}},
                {FlowBranchLogicalType.MasterHotfix, new[] {"hotfix", "^master"}}
            };
        }

        public IBranchingStrategy Strategy => _strategy;

        public IEnumerable<IBranch> EnumerateBranchesInLogicalOrder()
        {
            return _branchesInLogicalOrder.WrapAsEnumerable();
        }

        public Color GetSuggestedDrawingColorFor(IBranch branch)
        {
            FlowBranchLogicalType ft = _branchesToTypes[branch];
            return _typesToColors[ft];
        }

        public void Initialise(IEnumerable<IBranch> branches)
        {
            _branchesToTypes.Clear();
            _branchesInLogicalOrder.Clear();

            List<IBranch> allBranches = branches.ToList();

            _lesserBranchSelector.PreProcessAllBranches(allBranches, _workItemRegex);

            FlowBranchLogicalType[] allBranchLogicalTypes = GetAllFlowBranchLogicalTypes();
            foreach (FlowBranchLogicalType flowType in allBranchLogicalTypes)
            {
                var selectedBranches = new List<IBranch>(allBranches.Count / 2);

                Regex[] regices = GetRegicesFor(flowType);

                for (int i = allBranches.Count - 1; i >= 0; i--)
                {
                    IBranch b = allBranches[i];

                    bool isMatch = regices.Any(r => r.IsMatch(b.Label));
                    if (!isMatch)
                    {
                        continue;
                    }

                    selectedBranches.Add(b);
                    allBranches.RemoveAt(i);
                }

                AddBranchesAs(selectedBranches, flowType);
            }

            // Leftovers.
            AddBranchesAs(allBranches, FlowBranchLogicalType.WorkItem);
        }

        public bool IsAWorkItemBranch(IBranch branch)
        {
            return _branchesToTypes[branch] == FlowBranchLogicalType.WorkItem;
        }

        private void AddBranchesAs(IEnumerable<IBranch> branches, FlowBranchLogicalType figuredOutFlowType)
        {
            BranchSorterByName branchSorterByName = new BranchSorterByName();

            IBranch[] branchesSorted = branches.OrderBy(b => b, branchSorterByName).ToArray();

            foreach (IBranch branch in branchesSorted)
            {
                FlowBranchLogicalType typeToAddAs = figuredOutFlowType;
                if (typeToAddAs != FlowBranchLogicalType.WorkItem)
                {
                    bool isActuallyLesser = _lesserBranchSelector.IsLesserBranch(branch);
                    if (isActuallyLesser)
                    {
                        typeToAddAs = FlowBranchLogicalType.WorkItem;
                    }
                }

                _branchesToTypes.Add(branch, typeToAddAs);
                _branchesInLogicalOrder.Add(branch);
            }
        }

        private static FlowBranchLogicalType[] GetAllFlowBranchLogicalTypes()
        {
            return Enum.GetValues(typeof(FlowBranchLogicalType)).Cast<FlowBranchLogicalType>().ToArray();
        }

        private Regex[] GetRegicesFor(FlowBranchLogicalType flowType)
        {
            if (!_typesToRegices.TryGetValue(flowType, out ISet<string> regexStrings))
            {
                return new Regex[0];
            }

            Regex[] regices = regexStrings.Select(s => new Regex(s, RegexOptions.IgnoreCase)).ToArray();
            return regices;
        }
    }
}