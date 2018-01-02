using Autofac;

namespace GitWriter.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            IContainer container = builder.Build();

            Executor e = new Executor();
            e.Execute();
        }
    }
}
