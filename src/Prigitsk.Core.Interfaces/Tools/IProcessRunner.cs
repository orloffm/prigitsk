namespace Prigitsk.Core.Tools
{
    public interface IProcessRunner
    {
        /// <summary>
        ///     Executes the command with the given argument and return the whole output.
        /// </summary>
        string Execute(string command, string argument);
    }
}