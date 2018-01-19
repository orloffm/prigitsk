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
            builder.RegisterAssemblyTypes().AsImplementedInterfaces();
            var container = builder.Build();

            IExecutor e = container.Resolve<IExecutor>();
            e.Execute(args);
        }
    }
}