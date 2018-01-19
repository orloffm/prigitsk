using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Core;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Module = Autofac.Module;

namespace Prigitsk.Shared
{
    public class NLoggerModule : Module
    {
        private readonly NLogLoggerProvider _provider;

        public NLoggerModule()
        {
            _provider = new NLogLoggerProvider();
        }

        /// <summary>
        ///     Called when the module loads. As <see cref="CreateLogger" />
        ///     returns an <see cref="ILogger" />, we are able to register
        ///     this function as a creator if loggers.
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(CreateLogger).AsImplementedInterfaces();
        }

        /// <summary>
        ///     Creates an instance of <see cref="ILogger" />
        /// </summary>
        /// <param name="c">Not used.</param>
        /// <param name="p">
        ///     Parameters. Expects a Type parameter that specifies
        ///     the type for which the logger is resolved.
        /// </param>
        private ILogger CreateLogger(IComponentContext c, IEnumerable<Parameter> p)
        {
            Type parentType = p.TypedAs<Type>();

            ILogger logger = _provider.CreateLogger(parentType.FullName);
            return logger;
        }

        /// <summary>
        ///     Attaches a callback to the process of preparing for resolving items.
        /// </summary>
        protected override void AttachToComponentRegistration(
            IComponentRegistry componentRegistry,
            IComponentRegistration registration)
        {
            registration.Preparing += Registration_Preparing;
        }

        /// <summary>
        ///     The callback that is called when preparing to resolve an item.
        ///     A Type parameter with the owning Type is created,
        ///     but it is enabled only
        ///     if one of the arguments is an <see cref="ILogger" />.
        /// </summary>
        private void Registration_Preparing(object sender, PreparingEventArgs args)
        {
            Type forType = args.Component.Activator.LimitType;

            ResolvedParameter logParameter = new ResolvedParameter(
                IsLoggerArgumentPresent,
                (p, c) => LoggerCreator(c, forType));

            args.Parameters = args.Parameters.Union(new[] {logParameter});
        }

        private object LoggerCreator(IComponentContext c, Type forType)
        {
            object resolved = c.Resolve<ILogger>(TypedParameter.From(forType));
            return resolved;
        }

        private bool IsLoggerArgumentPresent(ParameterInfo p, IComponentContext c)
        {
            return p.ParameterType == typeof(ILogger);
        }
    }
}