using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using AutoMapper;
using FluentValidation;
using HomeCinema.Data.Infrastructure;
using ShoppingCart.Application.Service;
using ShoppingCart.Domain.Data.Infrastructure;
using ShoppingCart.Domain.Data.Repository;
using ShoppingCart.Domain.ShoppingCart.Entity;
using ShoppingCart.Infrastructure.Core;
using ShoppingCart.Infrastructure.Dependencies;

namespace ShoppingCart.Api.App_Start
{
    public class DependencyConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<ShoppingCartService>().As<IShoppingCartService>().InstancePerLifetimeScope();
            builder.RegisterType<AutofacResolver>().As<IResolver>();
            builder.RegisterType<Bus>().As<IBus>().InstancePerLifetimeScope();
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutoMapperProfile());
            });
            autoMapperConfig.CreateMapper();
            builder.Register(c => autoMapperConfig).AsSelf().SingleInstance();
            //builder.Register(c => c.Resolve<MapperConfiguration>().CreateMapper(c.Resolve))
            //    .As<IMapper>()
            //    .InstancePerLifetimeScope();
            var queryAssembly = typeof(Query.Queries.ListItemsQuery).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(queryAssembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(queryAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(queryAssembly).AsClosedTypesOf(typeof(IValidator<>));
          
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();

             var domainAssembly = typeof(Domain.ShoppingCart.Basket).GetTypeInfo().Assembly;
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(ICommandHandler<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsClosedTypesOf(typeof(IValidator<>));
            builder.RegisterAssemblyTypes(domainAssembly).AsImplementedInterfaces();
            builder.RegisterAssemblyTypes(typeof(Domain.ShoppingCart.EventHandler.AddedToBasketEventHandler).GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IEventHandler<>));

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<ShoppingCartConext>().As<DbContext>().InstancePerRequest();
            builder.RegisterType<DbFactory>().As<IDbFactory>().InstancePerRequest();
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerRequest();
            builder.RegisterGeneric(typeof(GenericRepository<>))
                .As(typeof(IGenericRepository<>))
                .InstancePerRequest();
            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));

            configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            //return container;
        }

    }
}