namespace Prigitsk.Core.Strategy
{
    /// <summary>
    ///     Knowledge about a regular expression that, if applicable
    ///     to the ending part of a branch name (except for some other branch name),
    ///     explicitly signalises that this branch exists for a single work item.
    /// </summary>
    public interface IWorkItemSuffixRegex
    {
        /// <summary>
        ///     The regular expression. May be null.
        /// </summary>
        string RegexString { get; }
    }
}