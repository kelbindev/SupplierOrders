using Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Context;

namespace SupplierOrders.DependencyInjection;

public static class ConfigureRepository
{
    public static IServiceCollection ConfigureRepositoryDependecy(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<SupplierOrdersContext>(opts => opts.UseSqlServer(configuration["ConnectionString:Db"]));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        return services;
    }
}
