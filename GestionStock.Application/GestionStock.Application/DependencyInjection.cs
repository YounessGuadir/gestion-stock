using Microsoft.Extensions.DependencyInjection;

using GestionStock.Application.Tenants.Commands.Create;
using GestionStock.Application.Tenants.Commands.Update;
using GestionStock.Application.Tenants.Queries.GetAll;
using GestionStock.Application.Tenants.Queries.GetById;

// Categories
using GestionStock.Application.Categories.Commands.Create;
using GestionStock.Application.Categories.Commands.Update;
using GestionStock.Application.Categories.Commands.Delete;
using GestionStock.Application.Categories.Queries.GetAll;
using GestionStock.Application.Categories.Queries.GetById;

// Products
using GestionStock.Application.Products.Commands.Create;
using GestionStock.Application.Products.Commands.Update;
using GestionStock.Application.Products.Commands.Delete;
using GestionStock.Application.Products.Queries.GetAll;
using GestionStock.Application.Products.Queries.GetById;

// Stock
using GestionStock.Application.Stock.Commands.AdjustStock;
using GestionStock.Application.Stock.Commands.DonateStock;
using GestionStock.Application.Stock.Queries.GetBalance;
using GestionStock.Application.Stock.Queries.GetMovements;

namespace GestionStock.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // -------- Tenants --------
        services.AddScoped<CreateTenantHandler>();
        services.AddScoped<UpdateTenantHandler>();
        services.AddScoped<GetAllTenantsHandler>();
        services.AddScoped<GetTenantByIdHandler>();

        // -------- Categories --------
        services.AddScoped<CreateCategoryHandler>();
        services.AddScoped<UpdateCategoryHandler>();
        services.AddScoped<DeleteCategoryHandler>();
        services.AddScoped<GetCategoriesHandler>();
        services.AddScoped<GetCategoryByIdHandler>();

        // -------- Products --------
        services.AddScoped<CreateProductHandler>();
        services.AddScoped<UpdateProductHandler>();
        services.AddScoped<DeleteProductHandler>();
        services.AddScoped<GetProductsHandler>();
        services.AddScoped<GetProductByIdHandler>();

        // -------- Stock --------
        services.AddScoped<AdjustStockHandler>();
        services.AddScoped<DonateStockHandler>();
        services.AddScoped<GetBalanceHandler>();
        services.AddScoped<GetMovementsHandler>();

        return services;
    }
}