using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace Prigitsk.Core.Git.LibGit2Sharp
{
    public sealed class CommitWrapped : ICommit
    {
        private readonly Commit _commit;

        private CommitWrapped(Commit commit)
        {
            _commit = commit;
        }

        public string Sha => _commit.Sha;
        public IEnumerable<ICommit> Parents => _commit.Parents.Select(Create);
        public ISignature Author => SignatureWrapped.Create(_commit.Author);
        public ISignature Committer => SignatureWrapped.Create(_commit.Committer);
        public string Message => _commit.Message;

        public static ICommit Create(Commit commit)
        {
            if (commit == null)
            {
                return null;
            }

            return new CommitWrapped(commit);
        }
    }
}