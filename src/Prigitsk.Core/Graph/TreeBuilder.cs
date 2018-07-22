using System.Collections.Generic;
using System.Linq;
using Prigitsk.Core.Entities;
using Prigitsk.Core.RepoData;
using Prigitsk.Core.Strategy;

namespace Prigitsk.Core.Graph
{
    public sealed class TreeBuilder : ITreeBuilder
    {
        private readonly IBranchPickerFactory _branchPickerFactory;
        private readonly ITagPickerFactory _tagPickerFactory;

        public TreeBuilder(ITagPickerFactory tagPickerFactory, IBranchPickerFactory branchPickerFactory)
        {
            _tagPickerFactory = tagPickerFactory;
            _branchPickerFactory = branchPickerFactory;
        }

        public ITree Build(
            IRepositoryData repository,
            IRemote remoteToUse,
            IBranchesKnowledge branchesKnowledge,
            IBranchPickingOptions branchPickingOptions,
            ITagPickingOptions tagPickingOptions)
        {
            ITree tree = new Tree();

            AddCommits(repository.Commits, tree);

            AddBranches(branchesKnowledge, repository.Commits, branchPickingOptions, tree);

            AddTags(repository.Tags, tagPickingOptions, tree);

            return tree;
        }

        private void AddBranches(
            IBranchesKnowledge branchesKnowledge,
            ICommitsData commitsData,
            IBranchPickingOptions branchPickingOptions,
            ITree tree)
        {
            // Branches in the writing order.
            IBranch[] branchesOrdered = branchesKnowledge.EnumerateBranchesInLogicalOrder().ToArray();

            IBranchPicker branchPicker = _branchPickerFactory.CreateBranchPicker(branchPickingOptions);

            // Filter them so that we get only those we want to write.
            IBranch[] branchesFiltered = branchesOrdered.Where(b => branchPicker.ShouldBePicked(b.Label)).ToArray();

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
        private void AddTags(IEnumerable<ITag> tags, ITagPickingOptions tagPickingOptions, ITree tree)
        {
            ITagInfo[] tagInfos = tags.Select(t => CreateTagInfo(t, tree)).ToArray();

            // Prepare picker.
            ITagPicker tagPicker = PrepareTagPicker(tagPickingOptions, tagInfos);

            // Now tags.
            foreach (ITagInfo tagInfo in tagInfos)
            {
                ITag tag = tagInfo.Tag;

                if (!tagPicker.CheckIfTagShouldBePicked(tag))
                {
                    continue;
                }

                tree.AddTag(tag);
            }
        }

        private ITagInfo CreateTagInfo(ITag tag, ITree tree)
        {
            INode node = tree.GetNode(tag.Tip);
            IBranch branch = tree.GetContainingBranch(node);
            return new TagInfo(tag, node, branch);
        }

        private ITagPicker PrepareTagPicker(
            ITagPickingOptions tagPickingOptions,
            IEnumerable<ITagInfo> tagInfos)
        {
            ITagPicker tagPicker = _tagPickerFactory.CreateTagPicker(tagPickingOptions);

            tagPicker.PreProcessAllTags(tagInfos);
            return tagPicker;
        }
    }

    public sealed class TagInfo : ITagInfo
    {
        public TagInfo(ITag tag, INode node, IBranch branch)
        {
            Tag = tag;
            Node = node;
            ContainingBranch = branch;
        }

        public IBranch ContainingBranch { get; }

        public INode Node { get; }

        public ITag Tag { get; }
    }
}