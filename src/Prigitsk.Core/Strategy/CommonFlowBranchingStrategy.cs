using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Prigitsk.Core.Entities;
using Prigitsk.Core.Entities.Comparers;

namespace Prigitsk.Core.Strategy
{
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

        public string GetHexColorFor(IBranch branch)
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

        public IEnumerable<IBranch> SortByPriorityDescending(IEnumerable<IBranch> branchesEnumerable)
        {
            var branchesLeftToBeSorted = new List<IBranch>(branchesEnumerable);
            BranchSorterByName branchSorterByName = new BranchSorterByName();
            foreach (string regexString in RegexStrings)
            {
                Regex r = new Regex(regexString, RegexOptions.IgnoreCase);
                var applicableTo = new List<IBranch>(branchesLeftToBeSorted.Count / 2);
                for (int i = branchesLeftToBeSorted.Count - 1; i >= 0; i--)
                {
                    IBranch b = branchesLeftToBeSorted[i];
                    if (!r.IsMatch(b.Label))
                    {
                        continue;
                    }

                    applicableTo.Add(b);
                    branchesLeftToBeSorted.RemoveAt(i);
                }

                applicableTo.Sort(branchSorterByName);
                foreach (IBranch b in applicableTo)
                {
                    yield return b;
                }
            }

            // Leftovers.
            branchesLeftToBeSorted.Sort(branchSorterByName);
            foreach (IBranch b in branchesLeftToBeSorted)
            {
                yield return b;
            }
        }

        public IEnumerable<IBranch> SortForWritingDescending(
            IEnumerable<IBranch> branchesEnumerable,
            IDictionary<IBranch, DateTimeOffset> firstNodeDates)
        {
            var branches = new List<IBranch>(branchesEnumerable);
            var ret = new List<IBranch>();
            IComparer<IBranch> byDateComparer = new BranchSorterByDate(firstNodeDates);
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

        private void AddEndingWith(
            string endString,
            ICollection<IBranch> source,
            ICollection<IBranch> target,
            IComparer<IBranch> sorter)
        {
            endString = endString.ToLower();
            AddWith(s => s.EndsWith(endString), source, target, sorter);
        }

        private void AddEqual(
            string value,
            ICollection<IBranch> source,
            ICollection<IBranch> target)
        {
            IBranch match = source.FirstOrDefault(z => z.Label == value);
            if (match != null)
            {
                target.Add(match);
                source.Remove(match);
            }
        }

        private void AddStartingWith(
            string startString,
            ICollection<IBranch> source,
            ICollection<IBranch> target,
            IComparer<IBranch> sorter)
        {
            startString = startString.ToLower();
            AddWith(s => s.StartsWith(startString), source, target, sorter);
        }

        private void AddWith(
            Func<string, bool> predicate,
            ICollection<IBranch> source,
            ICollection<IBranch> target,
            IComparer<IBranch> sorter)
        {
            List<IBranch> matching = source.Where(z => predicate(z.Label.ToLower())).ToList();
            matching.Sort(sorter);
            foreach (IBranch b in matching)
            {
                target.Add(b);
                source.Remove(b);
            }
        }

        private bool IsRelease(string label)
        {
            return label.StartsWith("release") || label.EndsWith("-rc");
        }
    }
}