using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Tests.StubEntities
{
    public sealed class HashStub : IHash
    {
        public HashStub()
        {
            
        }

        public HashStub(string hashValue) :this()
        {
            Treeish = hashValue;
            Value = hashValue;
        }

        public string Treeish { get; set; }

        public string Value { get; set; }

        public bool Equals(IHash other)
        {
            return ReferenceEquals(this, other);
        }

        public string ToShortString()
        {
            return Treeish;
        }
    }
}