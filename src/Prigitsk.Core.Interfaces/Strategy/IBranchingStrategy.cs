using System;
using System.Collections.Generic;
using Prigitsk.Core.Entities;

namespace Prigitsk.Core.Strategy
{
    /// <summary>
    ///     Represents a branching strategy applicable to the repository.
    /// </summary>
    public interface IBranchingStrategy
    {
        /// <summary>
        ///     Orders branches by their logical creation priority.
        /// </summary>
        IEnumerable<IBranch> SortByPriorityDescending(IEnumerable<IBranch> branchesEnumerable);

        /// <summary>
        ///     Orders branches by their importance.
        /// </summary>
        IEnumerable<IBranch> SortForWritingDescending(
            IEnumerable<IBranch> branchesEnumerable,
            IDictionary<IBranch, DateTimeOffset> firstNodeDates);

        /// <summary>
        ///     Returns HTML color for the branch. TODO: return an enum.
        /// </summary>
        string GetHtmlColorFor(IBranch branch);
    }
}