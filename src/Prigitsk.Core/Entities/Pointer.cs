using System.Diagnostics;

namespace Prigitsk.Core.Entities
{
    [DebuggerDisplay("{Name} => {Tip.ToShortString()}")]
    public abstract class Pointer : IPointer
    {
        protected Pointer(string name, IHash tip)
        {
            Tip = tip;
            Name = name;
        }

        public string Name { get; }

        public IHash Tip { get; }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}