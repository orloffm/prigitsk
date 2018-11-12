using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text.RegularExpressions;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Entities.Comparers;
using Prigitsk.Framework;

namespace Prigitsk.Core.Strategy
{
    public abstract class BranchesKnowledgeBase : IBranchesKnowledge
    {
        private readonly List<IBranch> _branchesInLogicalOrder;
        private readonly Dictionary<IBranch, BranchLogicalType> _branchesToTypes;
        private readonly IWorkItemBranchSelector _lesserBranchSelector;

        private readonly ILesserBranchRegex _workItemRegex;

        protected BranchesKnowledgeBase(
            ILesserBranchRegex workItemRegex,
            IWorkItemBranchSelectorFactory workItemBranchSelectorFactory
        )
        {
            _workItemRegex = workItemRegex;
            _lesserBranchSelector = workItemBranchSelectorFactory.MakeSelector();

            _branchesToTypes = new Dictionary<IBranch, BranchLogicalType>();
            _branchesInLogicalOrder = new List<IBranch>();
        }

        public IEnumerable<IBranch> EnumerateBranchesInLogicalOrder()
        {
            return _branchesInLogicalOrder.WrapAsEnumerable();
        }

        public Color GetSuggestedDrawingColorFor(IBranch branch)
        {
            BranchLogicalType ft = _branchesToTypes[branch];
            return GetColorInternal(ft);
        }

        public void Initialise(IEnumerable<IBranch> branches)
        {
            _branchesToTypes.Clear();
            _branchesInLogicalOrder.Clear();

            List<IBranch> allBranches = branches.ToList();

            _lesserBranchSelector.PreProcessAllBranches(allBranches, _workItemRegex);

            BranchLogicalType[] allBranchLogicalTypes = GetAllBranchLogicalTypes();
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

        public bool IsAWorkItemBranch(IBranch branch)
        {
            return _branchesToTypes[branch] == BranchLogicalType.WorkItem;
        }

        protected abstract Color GetColorInternal(BranchLogicalType ft);

        private void AddBranchesAs(IEnumerable<IBranch> branches, BranchLogicalType figuredOutFlowType)
        {
            BranchSorterByName branchSorterByName = new BranchSorterByName();

            IBranch[] branchesSorted = branches.OrderBy(b => b, branchSorterByName).ToArray();

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
            }
        }

        private static BranchLogicalType[] GetAllBranchLogicalTypes()
        {
            return Enum.GetValues(typeof(BranchLogicalType)).Cast<BranchLogicalType>().ToArray();
        }

        private Regex[] GetRegicesFor(BranchLogicalType flowType)
        {
            if (!TryGetRegexStringsInternal(flowType, out ISet<string> regexStrings))
            {
                return new Regex[0];
            }

            Regex[] regices = regexStrings.Select(s => new Regex(s, RegexOptions.IgnoreCase)).ToArray();
            return regices;
        }

        protected abstract bool TryGetRegexStringsInternal(BranchLogicalType flowType, out ISet<string> regexStrings);
    }
}