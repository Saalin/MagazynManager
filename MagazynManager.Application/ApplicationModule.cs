using Autofac;
using MagazynManager.Application.CommandHandlers;
using MagazynManager.Application.DataProviders;
using MagazynManager.Application.QueryHandlers;
using NodaTime;
using System;

namespace MagazynManager.Application
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
                .Where(t => Attribute.GetCustomAttribute(t, typeof(QueryHandlerAttribute)) != null)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterAssemblyTypes(typeof(ApplicationModule).Assembly)
                .Where(t => Attribute.GetCustomAttribute(t, typeof(CommandHandlerAttribute)) != null)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterType<InMemoryRefreshTokenStore>().AsImplementedInterfaces().SingleInstance();
            builder.Register(_ => SystemClock.Instance).As<IClock>();

            base.Load(builder);
        }
    }
}