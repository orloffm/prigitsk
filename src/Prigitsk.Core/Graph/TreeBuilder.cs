using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Graph
{
    public class TreeBuilder : ITreeBuilder
    {
        private readonly ITagPickerFactory _tagPickerFactory;

        public TreeBuilder(ITagPickerFactory tagPickerFactory)
        {
            _tagPickerFactory = tagPickerFactory;
        }

        public ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchingStrategy strategy,
            ITreeBuildingOptions options)
        {
            ITree tree = new Tree();

            AddCommits(repository, tree);

            AddBranches(repository, remoteToUse, strategy, options, tree);

            AddTags(repository, options, tree);

            return tree;
        }

        private static void AddBranches(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchingStrategy strategy,
            ITreeBuildingOptions options,
            ITree tree)
        {
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

                var hashesInBranch = new List<IHash>(commits.Count / 3);
                IEnumerable<ICommit> upTheTree = repository.Commits.EnumerateUpTheHistoryFrom(tip);
                foreach (ICommit commit in upTheTree)
                {
                    if (!commits.Contains(commit))
                    {
                        break;
                    }

                    hashesInBranch.Add(commit.Hash);
                    commits.Remove(commit);
                }

                hashesInBranch.Reverse();
                tree.AddBranch(b, hashesInBranch);
            }
        }

        private static void AddCommits(IRepositoryData repository, ITree tree)
        {
            // Commits.
            foreach (ICommit commit in repository.Commits)
            {
                tree.AddCommit(commit);
            }
        }

        /// <summary>
        ///     Adds tags, but only those that are permitted by the tag picker.
        /// </summary>
        private void AddTags(IRepositoryData repository, ITreeBuildingOptions options, ITree tree)
        {
            Tuple<ITag, INode>[] tagsAndNodes = repository.Tags
                .Select(t => Tuple.Create(t, tree.GetNode(t.Tip)))
                .ToArray();

            // Prepare picker.
            ITagPicker tagPicker = PrepareTagPicker(options, tagsAndNodes);

            // Now tags.
            foreach (Tuple<ITag, INode> tagAndNode in tagsAndNodes)
            {
                ITag tag = tagAndNode.Item1;
                IBranch branch = tree.GetContainingBranch(tagAndNode.Item2);

                if (!tagPicker.CheckIfTagShouldBePicked(tag, branch))
                {
                    continue;
                }

                tree.AddTag(tag);
            }
        }

        private ITagPicker PrepareTagPicker(ITreeBuildingOptions options, IEnumerable<Tuple<ITag, INode>> tagsAndNodes)
        {
            IEnumerable<Tuple<ITag, ICommit>> commitsTuple =
                tagsAndNodes.Select(t => Tuple.Create(t.Item1, t.Item2.Commit));

            ITagPicker tagPicker = _tagPickerFactory.CreateTagPicker(options.TagPickingOptions);
            tagPicker.PreProcessAllTags(commitsTuple);
            return tagPicker;
        }
    }
}