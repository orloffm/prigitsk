using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Prigitsk.Core.Graph.Strategy
{
    [Obsolete]
    public sealed class CommonFlowBranchingStrategy : IBranchingStrategy
    {
        private static readonly string[] RegexStrings =
        {
            "^master$",
            "^develop?",
            "^dev",
            "-dev-",
            "^release",
            "-RC$",
            "^hotfix"
        };

        private void AddEndingWith(
            string endString,
            ICollection<OriginBranch> source,
            ICollection<OriginBranch> target,
            IComparer<OriginBranch> sorter)
        {
            endString = endString.ToLower();
            AddWith(s => s.EndsWith(endString), source, target, sorter);
        }

        private void AddEqual(
            string value,
            ICollection<OriginBranch> source,
            ICollection<OriginBranch> target)
        {
            OriginBranch match = source.FirstOrDefault(z => z.Label == value);
            if (match != null)
            {
                target.Add(match);
                source.Remove(match);
            }
        }

        private void AddStartingWith(
            string startString,
            ICollection<OriginBranch> source,
            ICollection<OriginBranch> target,
            IComparer<OriginBranch> sorter)
        {
            startString = startString.ToLower();
            AddWith(s => s.StartsWith(startString), source, target, sorter);
        }

        private void AddWith(
            Func<string, bool> predicate,
            ICollection<OriginBranch> source,
            ICollection<OriginBranch> target,
            IComparer<OriginBranch> sorter)
        {
            List<OriginBranch> matching = source.Where(z => predicate(z.Label.ToLower())).ToList();
            matching.Sort(sorter);
            foreach (OriginBranch b in matching)
            {
                target.Add(b);
                source.Remove(b);
            }
        }

        public string GetHtmlColorFor(OriginBranch branch)
        {
            string label = branch.Label.ToLower();
            if (label == "master")
            {
                return "#27E4F9";
            }

            if (label == "develop")
            {
                return "#FFE333";
            }

            if (label.StartsWith("hotfix"))
            {
                return "#FD5965";
            }

            if (IsRelease(label))
            {
                return "#52C322";
            }

            return "#FB3DB5";
        }

        private bool IsRelease(string label)
        {
            return label.StartsWith("release") || label.EndsWith("-rc");
        }

        public IEnumerable<OriginBranch> SortByPriority(IEnumerable<OriginBranch> branchesEnumerable)
        {
            var branchesLeftToBeSorted = new List<OriginBranch>(branchesEnumerable);
            BranchSorterByName branchSorterByName = new BranchSorterByName();
            foreach (string regexString in RegexStrings)
            {
                Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
                var applicableTo = new List<OriginBranch>(branchesLeftToBeSorted.Count / 2);
                for (int i = branchesLeftToBeSorted.Count - 1; i >= 0; i--)
                {
                    OriginBranch b = branchesLeftToBeSorted[i];
                    if (!r.IsMatch(b.Label))
                    {
                        continue;
                    }

                    applicableTo.Add(b);
                    branchesLeftToBeSorted.RemoveAt(i);
                }

                applicableTo.Sort(branchSorterByName);
                foreach (OriginBranch b in applicableTo)
                {
                    yield return b;
                }
            }

            // Leftovers.
            branchesLeftToBeSorted.Sort(branchSorterByName);
            foreach (OriginBranch b in branchesLeftToBeSorted)
            {
                yield return b;
            }
        }

        public IEnumerable<OriginBranch> SortForWriting(
            IEnumerable<OriginBranch> branchesEnumerable,
            Dictionary<OriginBranch, DateTime> firstNodeDates)
        {
            var branches = new List<OriginBranch>(branchesEnumerable);
            var ret = new List<OriginBranch>();
            IComparer<OriginBranch> byDateComparer = new BranchSorterByDate(firstNodeDates);
            // master, hotfixes sorted, releases sorted, develop, others sorted.
            // Sort is by first node's date.
            AddEqual("master", branches, ret);
            AddStartingWith("hotfix", branches, ret, byDateComparer);
            AddStartingWith("release", branches, ret, byDateComparer);
            AddEndingWith("-RC", branches, ret, byDateComparer);
            AddEqual("develop", branches, ret);
            AddStartingWith("", branches, ret, byDateComparer);
            return ret;
        }
    }
}