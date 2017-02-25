using System;
using System.Collections.Generic;
using System.Linq;
using OrlovMikhail.GitTools.Loading.Client.Repository;
using OrlovMikhail.GitTools.Structure;

namespace OrlovMikhail.GitTools.Processing
{
    public class RepositoryProcessor : IRepositoryProcessor
    {
        private readonly IBranchingStrategy _branchingStrategy;

        public RepositoryProcessor(IBranchingStrategy branchingStrategy)
        {
            _branchingStrategy = branchingStrategy;
        }

        public IGraphState Process(IRepositoryState state, RepositoryProcessingOptions options)
        {
            IGraphState p = new GraphState();

            // Create a list of branch pointers.
            var branchSources = new Dictionary<string, CommitInfo>(StringComparer.OrdinalIgnoreCase);

            // First pass - add all commits to the graph and build the branch array.
            foreach (CommitInfo commitInfo in state.CommitInfos)
            {
                p.AddNode(commitInfo.Hash, commitInfo.Parents);

                foreach (string branchName in commitInfo.Branches)
                {
                    p.AddBranch(commitInfo.Hash, branchName);

                    branchSources.Add(branchName, commitInfo);
                }

                foreach (string tagName in commitInfo.Tags)
                {
                    p.AddTag(commitInfo.Hash, tagName);
                }
            }

            // Sorted branch names.
            string[] branchesSorted = _branchingStrategy.OrderBranchNames(branchSources.Keys).ToArray();

            // Now apply the branch to the nodes.
            foreach (string branch in branchesSorted)
            {
                CommitInfo source = branchSources[branch];

                IEnumerable<Node> treeUp = p.EnumerateUpFrom(source.Hash);
                foreach (Node node in treeUp)
                {
                    if (node.AssignedBranch != null)
                    {
                        // We go up till set branch name.
                        break;
                    }

                    p.AssignBranch(node.Hash, branch);
                }
            }

            return p;
        }
    }
}