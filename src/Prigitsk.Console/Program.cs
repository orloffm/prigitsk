using Autofac;
using Prigitsk.Shared;

namespace Prigitsk.Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<NLoggerModule>();
            builder.RegisterType<Executor>();
            var container = builder.Build();

            Executor e = container.Resolve<Executor>();
            e.Execute();
        }
    }
}