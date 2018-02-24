using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Tree
{
    public class TreeBuilder : ITreeBuilder
    {
        public ITree Build(IRepositoryData repository, IBranchingStrategy strategy, ITreeBuildingOptions options)
        {
            // First, get the remote.
            IRemote remote = PickRemote(repository, options);

            // Commits.
            ITree tree = new Tree();
            foreach (ICommit commit in repository.Commits)
            {
                tree.AddCommit(commit);
            }

            // All branches from this remote.
            IEnumerable<IBranch> branches = repository.Branches.GetFor(remote);

            // Filter them so that we get only those we want to write.
            IBranch[] branchesFiltered = branches.Where(b => options.CheckIfBranchShouldBePicked(b.Label)).ToArray();

            // Sort these branches.
            IBranch[] branchesSorted = strategy.SortByPriorityDescending(branchesFiltered).ToArray();

            // Add commits by branch.
            var commits = new HashSet<ICommit>(repository.Commits);
            foreach (IBranch b in branchesSorted)
            {
                ICommit tip = repository.Commits.GetByHash(b.Tip);

                var commitsInBranch = new List<ICommit>(commits.Count / 2);
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
                if (!options.CheckIfTagShouldBePicked(tag.Name))
                {
                    continue;
                }

                tree.AddTag(tag);
            }

            return tree;
        }

        private IRemote PickRemote(IRepositoryData repository, ITreeBuildingOptions options)
        {
            // We need to return something, so this check is mandatory.
            if (repository.Remotes.Count == 0)
            {
                throw new InvalidOperationException("The repository does not contain remotes.");
            }

            if (string.IsNullOrWhiteSpace(options.RemoteToUse))
            {
                // Use default.
                IRemote originRemote = repository.Remotes.GetRemoteByName("origin");

                if (originRemote != null)
                {
                    return originRemote;
                }

                // Do we have multiple?
                if (repository.Remotes.Count > 1)
                {
                    throw new InvalidOperationException(
                        "The repository contains multiple remotes, cannot pick default.");
                }

                return repository.Remotes.Single();
            }

            // Find matching remote by name.
            IRemote matching = repository.Remotes.GetRemoteByName(options.RemoteToUse);
            if (matching == null)
            {
                throw new InvalidOperationException(
                    $"The repository does not contain a remote named \"{options.RemoteToUse}\".");
            }

            return matching;
        }
    }
}