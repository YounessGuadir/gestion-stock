using GestionStock.Application.Abstractions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Products.Dtos;

namespace GestionStock.Application.Products.Queries.GetAll;

public class GetProductsHandler : IQueryHandler<GetProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;

    public GetProductsHandler(IProductRepository products, IStockBalanceRepository balances)
    {
        _products = products;
        _balances = balances;
    }


    public async Task<List<ProductDto>> Handle(GetProductsQuery query, CancellationToken ct)
    {
        var products = await _products.GetAllAsync(query.TenantId, query.CategoryId, query.IsActive, ct);

        // Simple version: load balance per product (later we optimize with JOIN in repo)
        var result = new List<ProductDto>();

        foreach (var p in products)
        {
            var balance = await _balances.GetByProductIdAsync(query.TenantId, p.Id, ct);

            result.Add(new ProductDto(
                p.Id, p.TenantId, p.CategoryId,
                p.Name, p.Description,
                p.Price, p.Unit, p.ImageUrl,
                p.IsActive,
                balance?.Quantity ?? 0,
                 p.Category?.Name ?? "-",
                p.CreatedAt, p.UpdatedAt
            ));
        }

        return result;
    }
}