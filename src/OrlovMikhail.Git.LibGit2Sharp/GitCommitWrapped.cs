using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace OrlovMikhail.Git.LibGit2Sharp
{
    public sealed class GitCommitWrapped : IGitCommit
    {
        private readonly Commit _commit;

        private GitCommitWrapped(Commit commit)
        {
            _commit = commit;
        }

        public IGitSignature Author => GitSignatureWrapped.Create(_commit.Author);

        public IGitSignature Committer => GitSignatureWrapped.Create(_commit.Committer);

        public string Message => _commit.Message;

        public IEnumerable<IGitCommit> Parents => _commit.Parents.Select(Create);

        public string Sha => _commit.Sha;

        public static IGitCommit Create(Commit commit)
        {
            if (commit == null)
            {
                return null;
            }

            return new GitCommitWrapped(commit);
        }
    }
}