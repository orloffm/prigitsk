using System.Collections.Generic;
using System.Drawing;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    public interface IBranchesKnowledge
    {
        /// <summary>
        ///     The branching strategy used to create this knowledge object.
        /// </summary>
        IBranchingStrategy Strategy { get; }

        /// <summary>
        ///     Returns all branches in their order.
        /// </summary>
        IEnumerable<IBranch> EnumerateBranchesInLogicalOrder();

        /// <summary>
        ///     Returns the color that should be used to draw the branch.
        ///     It is dark enough to be used on top of white background.
        /// </summary>
        Color? GetSuggestedDrawingColorFor(IBranch branch);

        /// <summary>
        ///     Returns whether the given branch is a per-workitem one.
        ///     For example, serving for a particular Jira number.
        ///     This may be used for drawing it differently.
        /// </summary>
        bool IsAWorkItemBranch(IBranch branch);
    }
}