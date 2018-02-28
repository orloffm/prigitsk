namespace Prigitsk.Console.General
{
    /// <summary>
    ///     Interface for a general class that handles execution.
    /// </summary>
    public interface IGeneralExecutor
    {
        int RunSafe(string[] args);
    }
}