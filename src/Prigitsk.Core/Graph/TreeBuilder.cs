using System;
using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Graph
{
    public sealed class TreeBuilder : ITreeBuilder
    {
        private readonly ITagPickerFactory _tagPickerFactory;

        public TreeBuilder(ITagPickerFactory tagPickerFactory)
        {
            _tagPickerFactory = tagPickerFactory;
        }

        public ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchesKnowledge branchesKnowledge,
            ITreeBuildingOptions options)
        {
            ITree tree = new Tree();

            AddCommits(repository.Commits, tree);

            AddBranches(branchesKnowledge, repository.Commits, options, tree);

            AddTags(repository.Tags, options, tree);

            return tree;
        }

        private static void AddBranches(
            IBranchesKnowledge branchesKnowledge,
            ICommitsData commitsData,
            ITreeBuildingOptions options,
            ITree tree)
        {
            // Sort these branches.

            // Branches in the writing order.
            IBranch[] branchesOrdered = branchesKnowledge.EnumerateBranchesInLogicalOrder().ToArray();

            // Filter them so that we get only those we want to write.
            IBranch[] branchesFiltered =
                branchesOrdered.Where(b => options.CheckIfBranchShouldBePicked(b.Label)).ToArray();

            var commitsSet = new HashSet<ICommit>(commitsData);
            foreach (IBranch b in branchesFiltered)
            {
                ICommit tip = commitsData.GetByHash(b.Tip);

                var hashesInBranch = new List<IHash>(commitsSet.Count / 3);
                IEnumerable<ICommit> upTheTree = commitsData.EnumerateUpTheHistoryFrom(tip);
                foreach (ICommit commit in upTheTree)
                {
                    if (!commitsSet.Contains(commit))
                    {
                        break;
                    }

                    hashesInBranch.Add(commit.Hash);
                    commitsSet.Remove(commit);
                }

                hashesInBranch.Reverse();
                tree.AddBranch(b, hashesInBranch);
            }
        }

        private static void AddCommits(IEnumerable<ICommit> commits, ITree tree)
        {
            // Commits.
            foreach (ICommit commit in commits)
            {
                tree.AddCommit(commit);
            }
        }

        /// <summary>
        ///     Adds tags, but only those that are permitted by the tag picker.
        /// </summary>
        private void AddTags(IEnumerable<ITag> tags, ITreeBuildingOptions options, ITree tree)
        {
            Tuple<ITag, INode>[] tagsAndNodes = tags
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