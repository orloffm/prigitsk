namespace Prigitsk.Core.Strategy
{
    /// <summary>
    ///     If this applies to a branch name, it is considered a lesser one and is drawn differently.
    /// </summary>
    public interface ILesserBranchRegex
    {
        /// <summary>
        ///     The regular expression. May be null.
        /// </summary>
        string RegexString { get; }
    }
}