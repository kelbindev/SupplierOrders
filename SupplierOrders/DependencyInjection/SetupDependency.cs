using Contracts.Repository;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Context;
using Service;
using Service.Contracts;

namespace SupplierOrders.DependencyInjection;

public static class SetupDependency
{
    public static IServiceCollection ConfigureRepository(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<SupplierOrdersContext>(opts => opts.UseSqlServer(configuration["ConnectionString:Db"]));
        services.AddScoped<IRepositoryManager, RepositoryManager>();
        return services;
    }

    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();
        return services;
    }
}
