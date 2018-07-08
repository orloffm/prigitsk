namespace Prigitsk.Core.Strategy
{
    public sealed class WorkItemSuffixRegex : IWorkItemSuffixRegex
    {
        public WorkItemSuffixRegex(string regex)
        {
            RegexString = regex;
        }

        public string RegexString { get; }
    }
}