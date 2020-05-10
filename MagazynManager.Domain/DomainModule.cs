using Autofac;
using MagazynManager.Domain.DomainServices;
using System;

namespace MagazynManager.Application
{
    public class DomainModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(DomainModule).Assembly)
                .Where(t => Attribute.GetCustomAttribute(t, typeof(DomainServiceAttribute)) != null)
                .InstancePerLifetimeScope()
                .AsSelf()
                .AsImplementedInterfaces();

            base.Load(builder);
        }
    }
}