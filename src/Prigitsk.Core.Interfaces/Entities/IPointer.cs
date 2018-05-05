using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Entities
{
    public interface IPointer : ITreeish
    {
        /// <summary>
        ///     Full name of the pointer.
        /// </summary>
        string FullName { get; }

        /// <summary>
        ///     Short name. For branches - the name without the remote part.
        /// </summary>
        string Label { get; }

        IHash Tip { get; }
    }
}