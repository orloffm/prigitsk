namespace Prigitsk.Core.Simplification
{
    public sealed class SimplificationOptions
        : ISimplificationOptions
    {
      
        public static SimplificationOptions None => new SimplificationOptions();

        public bool RemoveOrphans { get; set; }
    }
}