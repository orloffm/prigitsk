using System.Linq;
using Moq;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tests.Helpers
{
    public static class EH
    {
        public static IBranch MockBranch(string s, IHash hash)
        {
            return Mock.Of<IBranch>(
                b => b.RemoteName == "origin"
                     && b.Tip == hash
                     && b.FullName == "origin/" + s
                     && b.Label == s);
        }

        public static ICommit MockCommit(string hashValue, params ICommit[] parents)
        {
            IHash hash = Mock.Of<IHash>(h => h.Value == hashValue);
            IHash[] parentHashes = parents.Select(c => c.Hash).ToArray();
            ICommit commit = Mock.Of<ICommit>(c => c.Hash == hash && c.Parents == parentHashes);
            return commit;
        }
    }
}