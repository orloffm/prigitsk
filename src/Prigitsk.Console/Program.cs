using System.Reflection;
using Autofac;
using Prigitsk.Console.General;
using Prigitsk.Core.Nodes.Loading;

namespace Prigitsk.Console
{
    internal class Program
    {
        private static int Main(string[] args)
        {
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule<NLoggerModule>();
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(NodeLoader).Assembly).AsImplementedInterfaces();
            IContainer container = builder.Build();

            IGeneralExecutor exec = container.Resolve<IGeneralExecutor>();
            int exitCode = exec.RunSafe(args);

            return exitCode;
        }
    }
}