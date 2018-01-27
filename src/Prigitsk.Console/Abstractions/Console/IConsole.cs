namespace Prigitsk.Console.Abstractions.Console
{
    public interface IConsole
    {
        void WriteLine();
        void WriteLine(string format, params object[] arg);
        void WriteLine(string text);
        void Write(string text);
        string ReadLine();
    }
}