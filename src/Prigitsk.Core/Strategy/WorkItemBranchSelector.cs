using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    public sealed class WorkItemBranchSelector : IWorkItemBranchSelector
    {
        /// <summary>
        ///     The explicit list of lesser branches.
        /// </summary>
        private readonly HashSet<string> _lesserBranches;

        public WorkItemBranchSelector()
        {
            _lesserBranches = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        }

        public bool IsLesserBranch(IBranch branch)
        {
            return _lesserBranches.Contains(branch.Label);
        }

        public void PreProcessAllBranches(IEnumerable<IBranch> branches, IWorkItemSuffixRegex lesserBranchesRegex)
        {
            _lesserBranches.Clear();

            Regex r;
            if (!TryCreateRegex(lesserBranchesRegex, out r))
            {
                return;
            }

            PopulateLesserBranchesList(branches, r);
        }

        private void PopulateLesserBranchesList(IEnumerable<IBranch> branches, Regex r)
        {
            string[] branchNames = branches
                .Select(b => b.Label)
                .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            for (int i = 1; i < branchNames.Length; i++)
            {
                string branchName = branchNames[i];

                bool everSawAParent = false;

                for (int p = i - 1; p >= 0; p--)
                {
                    string possibleParentBranchName = branchNames[p];
                    bool isParent = branchName.StartsWith(possibleParentBranchName);
                    if (!isParent)
                    {
                        if (everSawAParent)
                        {
                            // There will be no more parents as the array is sorted.
                            break;
                        }

                        // There still may be parents before. This can be optimised further.
                        continue;
                    }

                    // Remember for future.
                    everSawAParent = true;

                    string suffix = branchName.Substring(possibleParentBranchName.Length);
                    bool suffixDoesMatch = r.IsMatch(suffix);
                    if (!suffixDoesMatch)
                    {
                        // Nope, try next one.
                        continue;
                    }

                    // OK, this is a lesser branch. Remember it and try the next one.
                    _lesserBranches.Add(branchName);
                    break;
                }
            }
        }

        private bool TryCreateRegex(IWorkItemSuffixRegex regexObject, out Regex regex)
        {
            regex = null;

            string regexString = regexObject?.RegexString;

            if (string.IsNullOrWhiteSpace(regexString))
            {
                return false;
            }

            try
            {
                regex = new Regex(regexString);
            }
            catch
            {
                regex = null;
                return false;
            }

            return true;
        }
    }
}