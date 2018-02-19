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

            // All branches from this remote.
            IEnumerable<IBranch> branches = repository.Branches.GetFor(remote);

            // Sort these branches.
            IBranch[] branchesSorted = strategy.SortByPriorityDescending(branches).ToArray();

            ITree tree = new Tree();

            var unprocessedNodes = new HashSet<ICommit>(repository.Commits);

            foreach (IBranch b in branchesSorted)
            {
                ICommit tip = repository.Commits.GetByHash(b.Tip);

                var nodesInBranch = new List<ICommit>(unprocessedNodes.Count / 2);
                IEnumerable<ICommit> upTheTree = repository.Commits.EnumerateUpTheHistoryFrom(tip);
                foreach (ICommit commit in upTheTree)
                {
                    if (!unprocessedNodes.Contains(commit))
                    {
                        break;
                    }

                    nodesInBranch.Add(commit);
                    unprocessedNodes.Remove(commit);
                }

                nodesInBranch.Reverse();
                //  tree.SetBranchNodes(b, nodesInBranch);
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