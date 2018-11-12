using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tests.StubEntities
{
    public sealed class BranchStub : IBranch
    {
        public BranchStub()
        {
        }

        public BranchStub(string label, IHash hash = null) : this()
        {
            RemoteName = "origin";
            Tip = hash;
            Treeish = hash?.Treeish;
            FullName = "origin/" + label;
            Label = label;
        }

        public string FullName { get; set; }

        public string Label { get; set; }

        public string RemoteName { get; set; }

        public IHash Tip { get; set; }

        public string Treeish { get; set; }

        public bool Equals(IBranch other)
        {
            return ReferenceEquals(this, other);
        }
    }
}