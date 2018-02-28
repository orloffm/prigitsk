using System.Diagnostics;

namespace Prigitsk.Core.Entities
{
    [DebuggerDisplay("{FullName} => {Tip.ToShortString()}")]
    public abstract class Pointer : IPointer
    {
        protected Pointer(string name, IHash tip)
        {
            Tip = tip;
            FullName = name;
        }

        public string FullName { get; }

        public virtual string Label => FullName;

        public IHash Tip { get; }

        public override int GetHashCode()
        {
            return FullName.GetHashCode();
        }
    }
}