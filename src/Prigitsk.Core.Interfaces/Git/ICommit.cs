using System.Collections.Generic;

namespace Prigitsk.Core.Git
{
    public interface ICommit:IGitObject
    {
        IEnumerable<ICommit> Parents { get; }
        ISignature Author { get; }
        ISignature Committer { get; }
        string Message { get; }
    }
}