namespace Prigitsk.Console.Abstractions.Console
{
    public interface IConsole
    {
        string ReadLine();

        void Write(string text);

        void WriteLine();

        void WriteLine(string format, params object[] arg);

        void WriteLine(string text);
    }
}