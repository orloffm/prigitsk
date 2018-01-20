namespace Prigitsk.Console.Verbs
{
    /// <summary>
    ///     Interface for classes that perform verb-based operations.
    /// </summary>
    public interface IVerbRunner
    {
        void Run();
    }

    public interface IVerbRunner<T> : IVerbRunner where T : IVerbRunnerOptions
    {
    }
}