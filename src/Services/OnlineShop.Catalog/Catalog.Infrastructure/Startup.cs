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

namespace Catalog.Infrastructure;

public static class Startup
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        MapsterSettings.Configure();
        return services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddServices();
    }


    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder builder, IConfiguration config) =>
        builder
            .UseStaticFiles()
            .UseFileStorage();
}