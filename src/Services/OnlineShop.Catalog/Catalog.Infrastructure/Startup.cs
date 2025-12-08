using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Mapster;
using MapsterMapper;
using Catalog.Infrastructure.Mapping;
using MediatR;
using Catalog.Infrastructure.Common;
using Catalog.Infrastructure.FileStorage;
using Catalog.Infrastructure.Repositories;
using Catalog.Domain.Core.SeedWork;

namespace Catalog.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddServices()
            .AddScoped(typeof(IReadRepository<>), typeof(ApplicationDbRepository<>))
            .AddScoped(typeof(IRepository<>), typeof(ApplicationDbRepository<>));
    }


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseStaticFiles()
            .UseFileStorage();
}