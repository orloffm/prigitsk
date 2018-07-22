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

        public void PreProcessAllBranches(IEnumerable<IBranch> branches, ILesserBranchRegex lesserBranchesRegex)
        {
            _lesserBranches.Clear();

            Regex r;
            if (!TryCreateRegex(lesserBranchesRegex, out r))
            {
                return;
            }

            string[] lesserBranches = branches.Select(b => b.Label).Where(b => r.IsMatch(b)).ToArray();
            foreach (string lesserBranch in lesserBranches)
            {
                _lesserBranches.Add(lesserBranch);
            }
        }

        private bool TryCreateRegex(ILesserBranchRegex regexObject, out Regex regex)
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