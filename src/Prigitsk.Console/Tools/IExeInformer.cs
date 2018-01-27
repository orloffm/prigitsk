namespace Prigitsk.Console.Abstractions
{
   public interface IExeInformer
   {
       bool TryFindFullPath(string exeName, out string fullPath);
   }
}
