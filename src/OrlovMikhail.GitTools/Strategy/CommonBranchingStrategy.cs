using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrlovMikhail.GitTools.Structure
{
    public class CommonBranchingStrategy : IBranchingStrategy
    {
        /// <summary>Orders branch names by priority according to the branching strategy.</summary>
        public IEnumerable<string> OrderBranchNames(IEnumerable<string> branches)
        {
            HashSet<string> branchesSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (string branch in branches)
                branchesSet.Add(branch);

            string[] prioritized = new string[] { "master", "hotfix", "develop", "dev", "release" };

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

            var theRestSorted = branchesSet.OrderBy(z => z);
            foreach (string leftOver in theRestSorted)
                yield return leftOver;
        }
    }
}
