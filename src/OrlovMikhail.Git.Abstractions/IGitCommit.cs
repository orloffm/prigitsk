using System.Collections.Generic;

namespace OrlovMikhail.Git
{
    public interface IGitCommit : IGitObject
    {
        IGitSignature Author { get; }

        IGitSignature Committer { get; }

        string Message { get; }

        IEnumerable<IGitCommit> Parents { get; }
    }
}