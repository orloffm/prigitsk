namespace GitWriter.Abstractions
{
    public interface IProcessRunner
    {
        string Execute(string command, string argument);
    }
}