namespace Prigitsk.Console.Verbs.Draw
{
    public class DrawRunnerOptions : IDrawRunnerOptions
    {
       public string Repository { get; }
       public string Target { get; }
       public string Output { get; }
       public string Format { get; }

        public DrawRunnerOptions(string repository=null, string target = null, string output = null, string format = null)
        {
            this.Repository = repository;
            this.Target = target;
            this.Output = output;
            this.Format = format;
        }
    }
}