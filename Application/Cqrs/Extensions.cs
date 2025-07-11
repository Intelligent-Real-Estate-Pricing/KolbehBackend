using Application.Cqrs.Behaviors;
using Application.Cqrs.Commands;
using Application.Cqrs.Commands.Dispatcher;
using Application.Cqrs.Queris;
using Application.Cqrs.Queris.Dispatcher;
using Data.Contracts;
using Data.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Cqrs
{
    public static class Extensions
    {
        public static IServiceCollection AddCqrs(this IServiceCollection services)
        {
            services.AddMediatR(c =>
            {
                c.RegisterServicesFromAssemblies(typeof(ICommand<>).Assembly);
                //register sequence is important
                c.AddOpenBehavior(typeof(ValidationBehavior<,>));
                c.AddOpenBehavior(typeof(CacheInvalidationBehavior<,>));
                c.AddOpenBehavior(typeof(TransactionBehavior<,>));
                c.AddOpenBehavior(typeof(CachingBehavior<,>));

            });
            services.AddValidatorsFromAssembly(typeof(ICommand<>).Assembly);
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICacheService, RedisCacheService>();

            return services;
        }
    }
}
