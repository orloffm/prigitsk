namespace Prigitsk.Core.Entities
{
    /// <summary>
    ///     Represents a remote branch.
    /// </summary>
    public interface IBranch : IPointer
    {
        /// <summary>
        ///     Name of the branch without the remote part.
        /// </summary>
        string Label { get; }

        /// <summary>
        ///     Name of the remote.
        /// </summary>
        string RemoteName { get; }

        bool Equals(IBranch other);
    }
}