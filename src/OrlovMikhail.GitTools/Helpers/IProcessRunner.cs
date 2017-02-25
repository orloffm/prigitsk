namespace OrlovMikhail.GitTools.Helpers
{
    public interface IProcessRunner
    {
        /// <summary>Runs the program and returns the full output.</summary>
        string Run(string executable, string arguments);
    }
}