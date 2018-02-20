using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Simplification
{
    public interface ISimplifier
    {
        /// <summary>
        /// Simplifies the tree.
        /// </summary>
        void Simplify(ITree tree, ISimplificationOptions options);
    }
}