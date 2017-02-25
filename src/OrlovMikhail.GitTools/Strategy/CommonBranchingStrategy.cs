using System;
using System.Collections.Generic;
using System.Linq;
using OrlovMikhail.GitTools.Common;

namespace OrlovMikhail.GitTools.Structure
{
    public class CommonBranchingStrategy : IBranchingStrategy
    {
        private readonly string[][] colorMappings =
        {
            new[] {"master", "#27E4F9"},
            new[] {"develop", "#FFE333"},
            new[] {"hotfix", "#FD5965"},
            new[] {"release", "#52C322"}
        };

        private readonly string fallBackColorHtml = "#C982AF";

        /// <summary>Orders branch names by priority according to the branching strategy.</summary>
        public IEnumerable<string> OrderBranchNames(IEnumerable<string> branches)
        {
            var branchesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string branch in branches)
            {
                branchesSet.Add(branch);
            }

            string[] prioritized = {"master", "hotfix", "develop", "dev", "release"};

            foreach (string item in prioritized)
            {
                if (branchesSet.Contains(item))
                {
                    yield return item;
                    branchesSet.Remove(item);
                }

                string[] startingFrom = branchesSet
                    .Where(z => z.StartsWith(item, StringComparison.OrdinalIgnoreCase))
                    .OrderBy(z => z)
                    .ToArray();

                foreach (string subItem in startingFrom)
                {
                    yield return subItem;
                    branchesSet.Remove(subItem);
                }
            }

            IOrderedEnumerable<string> theRestSorted = branchesSet.OrderBy(z => z);
            foreach (string leftOver in theRestSorted)
            {
                yield return leftOver;
            }
        }

        public Color GetColorForBranch(string branchName)
        {
            string nameLower = branchName.ToLower();

            foreach (string[] pair in colorMappings)
            {
                if (nameLower.StartsWith(pair[0]))
                {
                    return Color.FromHTML(pair[1]);
                }
            }

            return Color.FromHTML(fallBackColorHtml);
        }
    }
}