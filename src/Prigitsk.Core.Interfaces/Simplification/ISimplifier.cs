using Prigitsk.Core.Graph;

namespace Prigitsk.Core.Simplification
{
    public interface ISimplifier
    {
        /// <summary>
        ///     Simplifies the tree.
        /// </summary>
        void Simplify(ITree tree, ISimplificationOptions options);
    }
}