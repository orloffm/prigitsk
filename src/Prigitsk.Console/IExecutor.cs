namespace Prigitsk.Console
{
    public interface IExecutor
    {
        /// <summary>
        ///     Executes the process according to command line arguments.
        /// </summary>
        void Execute(string[] args);
    }
}