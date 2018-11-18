using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Entities.Comparers;
using Prigitsk.Framework;

namespace Prigitsk.Core.Strategy
{
    public sealed class BranchesKnowledge : IBranchesKnowledge
    {
        private readonly List<IBranch> _branchesInLogicalOrder;
        private readonly Dictionary<IBranch, BranchLogicalType> _branchesToTypes;
        private readonly IBranchesColorsAndRegices _colorsAndRegices;
        private readonly IWorkItemBranchSelector _lesserBranchSelector;
        private readonly ILogger _logger;
        private readonly ILesserBranchRegex _workItemRegex;

        public BranchesKnowledge(
            ILesserBranchRegex workItemRegex
            , IWorkItemBranchSelectorFactory workItemBranchSelectorFactory
            , IBranchesColorsAndRegices colorsAndRegices
            , IEnumerable<IBranch> branches
            , ILogger<BranchesKnowledge> logger
        )
        {
            _workItemRegex = workItemRegex;
            _colorsAndRegices = colorsAndRegices;
            _logger = logger;
            _lesserBranchSelector = workItemBranchSelectorFactory.MakeSelector();

            _branchesToTypes = new Dictionary<IBranch, BranchLogicalType>();
            _branchesInLogicalOrder = new List<IBranch>();

            Initialise(branches);
        }

        public IEnumerable<IBranch> EnumerateBranchesInLogicalOrder()
        {
            return _branchesInLogicalOrder.WrapAsEnumerable();
        }

        public Color GetSuggestedDrawingColorFor(IBranch branch)
        {
            BranchLogicalType ft = _branchesToTypes[branch];
            return _colorsAndRegices.GetColor(ft);
        }

        public bool IsAWorkItemBranch(IBranch branch)
        {
            return _branchesToTypes[branch] == BranchLogicalType.WorkItem;
        }

        private void AddBranchesAs(IEnumerable<IBranch> branches, BranchLogicalType figuredOutFlowType)
        {
            BranchSorterByName branchSorterByName = new BranchSorterByName();

            IBranch[] branchesSorted = branches.OrderBy(b => b, branchSorterByName)
                .ToArray();

            foreach (IBranch branch in branchesSorted)
            {
                BranchLogicalType typeToAddAs = figuredOutFlowType;
                if (typeToAddAs != BranchLogicalType.WorkItem)
                {
                    bool isActuallyLesser = _lesserBranchSelector.IsLesserBranch(branch);
                    if (isActuallyLesser)
                    {
                        typeToAddAs = BranchLogicalType.WorkItem;
                    }
                }

                _branchesToTypes.Add(branch, typeToAddAs);
                _branchesInLogicalOrder.Add(branch);

                _logger.Debug("{0} added as {1}.", branch, typeToAddAs);
            }
        }

        private Regex[] GetRegicesFor(BranchLogicalType flowType)
        {
            if (!_colorsAndRegices.TryGetRegexStringsInternal(flowType, out ISet<string> regexStrings))
            {
                return new Regex[0];
            }

            Regex[] regices = regexStrings.Select(s => new Regex(s, RegexOptions.IgnoreCase))
                .ToArray();
            return regices;
        }

        private void Initialise(IEnumerable<IBranch> branches)
        {
            _branchesToTypes.Clear();
            _branchesInLogicalOrder.Clear();

            List<IBranch> allBranches = branches.ToList();

            _lesserBranchSelector.PreProcessAllBranches(allBranches, _workItemRegex);

            BranchLogicalType[] allBranchLogicalTypes = _colorsAndRegices.GetAllBranchLogicalTypesOrdered();

            foreach (BranchLogicalType flowType in allBranchLogicalTypes)
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
            AddBranchesAs(allBranches, BranchLogicalType.WorkItem);
        }
    }
}