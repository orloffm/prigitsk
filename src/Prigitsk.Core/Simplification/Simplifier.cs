using Prigitsk.Core.Tree;

namespace Prigitsk.Core.Simplification
{
    public sealed class Simplifier : ISimplifier
    {
        public void Simplify(ITree tree, ISimplificationOptions options)
        {
            if (options.RemoveUntaggedOrphans)
            {
                RemoveUntaggedOrphans(tree);
            }
        }

        private void RemoveUntaggedOrphans(ITree tree)
        {
            throw new System.NotImplementedException();
        }
    }
}