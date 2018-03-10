namespace Prigitsk.Core.Tools
{
    public interface IProcessRunner
    {
        /// <summary>
        ///     Executes the command with the given argument and return the whole output.
        /// </summary>
        int Execute(string command, string argument);
    }
}