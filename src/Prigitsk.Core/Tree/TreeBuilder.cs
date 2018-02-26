using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Tree
{
    public class TreeBuilder : ITreeBuilder
    {
        public ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchingStrategy strategy,
            ITreeBuildingOptions options)
        {
            ITree tree = new Tree();

            // Commits.
            foreach (ICommit commit in repository.Commits)
            {
                tree.AddCommit(commit);
            }

            // All branches from this remote.
            IEnumerable<IBranch> branches = repository.Branches.GetFor(remoteToUse);

            // Filter them so that we get only those we want to write.
            IBranch[] branchesFiltered = branches.Where(b => options.CheckIfBranchShouldBePicked(b.Label)).ToArray();

            // Sort these branches.

            // Branches.
            IBranch[] branchesSorted = strategy.SortByPriorityDescending(branchesFiltered).ToArray();
            var commits = new HashSet<ICommit>(repository.Commits);
            foreach (IBranch b in branchesSorted)
            {
                ICommit tip = repository.Commits.GetByHash(b.Tip);

                var commitsInBranch = new List<ICommit>(commits.Count / 3);
                IEnumerable<ICommit> upTheTree = repository.Commits.EnumerateUpTheHistoryFrom(tip);
                foreach (ICommit commit in upTheTree)
                {
                    if (!commits.Contains(commit))
                    {
                        break;
                    }

                    commitsInBranch.Add(commit);
                    commits.Remove(commit);
                }

                commitsInBranch.Reverse();
                tree.AddBranchWithCommits(b, commitsInBranch);
            }

            // Now tags.
            foreach (ITag tag in repository.Tags)
            {
                if (!options.CheckIfTagShouldBePicked(tag.FullName))
                {
                    continue;
                }

                tree.AddTag(tag);
            }

            return tree;
        }
    }
}