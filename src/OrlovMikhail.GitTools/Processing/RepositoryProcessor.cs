using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public IProcessedRepository Process(IRepositoryState state,  RepositoryProcessingOptions options)
        {
            IProcessedRepository p = new ProcessedRepository();

            var allCommits = state.CommitInfos;

            // Create a list of branch pointers.
            Dictionary<string, CommitInfo> branchSources = new Dictionary<string, CommitInfo>(StringComparer.OrdinalIgnoreCase);
            foreach (CommitInfo commitInfo in allCommits)
            {
                foreach (string branchName in commitInfo.Branches)
                {
                    branchSources.Add(branchName, commitInfo);
                }
            }

            // Sorted branch names.
            string[] branchesSorted = _branchingStrategy.OrderBranchNames(branchSources.Keys).ToArray();
            foreach (string branch in branchesSorted)
            {
                CommitInfo source = branchSources[branch];

                while (source.Parents.Length > 0)
                {
                    // Get parent and set its branch.
                }
            }

            return p;
        }
    }
}
