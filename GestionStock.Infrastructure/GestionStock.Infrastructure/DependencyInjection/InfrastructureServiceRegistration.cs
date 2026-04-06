using GestionStock.Application.Abstractions;
using GestionStock.Application.Contracts.Persistence;

using GestionStock.Application.Security;

using GestionStock.Infrastructure.Persistence;
using GestionStock.Infrastructure.Repositories;
using GestionStock.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GestionStock.Infrastructure.DependencyInjection;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Repositories
        services.AddScoped<ITenantRepository, TenantRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IStockBalanceRepository, StockBalanceRepository>();
        services.AddScoped<IStockMovementRepository, StockMovementRepository>();


        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        // Security ✅
        services.AddScoped<IPasswordHasher, PasswordHasher>();


      

        return services;
    }
}
