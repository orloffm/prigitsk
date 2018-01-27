namespace Prigitsk.Console.Abstractions
{
    /// <summary>
    ///     Provides abstractions over system environment class.
    /// </summary>
    public interface IWindowsEnvironment
    {
        string GetEnvironmentVariable(string variable);
    }
}