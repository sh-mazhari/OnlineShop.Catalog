using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;

namespace Catalog.API.Configurations
{
    public static class MainSetup
    {
        public static void RegisterDI(this IServiceCollection services, IConfiguration configuration)
        {
            DatabaseSetup.AddDatabaseSetup(services, configuration);
            RepositoriesSetup.AddRepositorySetup(services);
            ServicesSetup.AddServicesSetup(services);
        }
    }
}
