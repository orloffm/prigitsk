namespace Prigitsk.Console.Tools
{
    public interface IExeInformer
    {
        bool TryFindFullPath(string exeName, out string fullPath);
    }
}