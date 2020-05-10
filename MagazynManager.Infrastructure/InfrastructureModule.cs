using Autofac;
using MagazynManager.Infrastructure.Authorization;
using MagazynManager.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using System;

namespace MagazynManager.Infrastructure
{
    public class InfrastructureModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DbConnectionSource>().As<IDbConnectionSource>().SingleInstance();

            builder.RegisterAssemblyTypes(typeof(InfrastructureModule).Assembly)
                .Where(t => Attribute.GetCustomAttribute(t, typeof(RepositoryAttribute)) != null)
                .InstancePerLifetimeScope()
                .AsImplementedInterfaces();

            builder.RegisterType<PermissionPolicyProvider>().As<IAuthorizationPolicyProvider>().SingleInstance();
            builder.RegisterType<PermissionHandler>().As<IAuthorizationHandler>().SingleInstance();

            base.Load(builder);
        }
    }
}