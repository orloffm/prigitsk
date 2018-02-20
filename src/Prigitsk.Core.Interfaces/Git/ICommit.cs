using System.Collections.Generic;

namespace Prigitsk.Core.Git
{
    public interface ICommit : IGitObject
    {
        ISignature Author { get; }

        ISignature Committer { get; }

        string Message { get; }

        IEnumerable<ICommit> Parents { get; }
    }
}