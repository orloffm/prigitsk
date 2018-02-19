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

        public IHash Tip { get; }
        public string Name { get; }
    }
}