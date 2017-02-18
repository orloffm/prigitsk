using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace ConGitWriter
{
    class Program
    {

        static void Main(string[] args)
        {
            ConfigureLog4Net();

            ContainerBuilder builder = new ContainerBuilder();
            RegisterClasses(builder);
            IContainer container = builder.Build();

            IWorker w = container.Resolve<IWorker>();
            w.Run(args);
        }

        private static void ConfigureLog4Net()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        private static void RegisterClasses(ContainerBuilder builder)
        {
            builder.RegisterType<Worker>().As<IWorker>();
        }
    }
}
