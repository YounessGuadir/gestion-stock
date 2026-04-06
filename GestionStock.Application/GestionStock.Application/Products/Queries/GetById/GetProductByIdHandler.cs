using GestionStock.Application.Abstractions;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Products.Dtos;

namespace GestionStock.Application.Products.Queries.GetById;

public class GetProductByIdHandler : IQueryHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;

    public GetProductByIdHandler(IProductRepository products, IStockBalanceRepository balances)
    {
        _products = products;
        _balances = balances;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(query.TenantId, query.Id, ct);
        if (product is null)
            throw new NotFoundException("Product not found.");

        var balance = await _balances.GetByProductIdAsync(query.TenantId, product.Id, ct);

        return new ProductDto(
            product.Id, product.TenantId, product.CategoryId,
            product.Name, product.Description,
            product.Price, product.Unit, product.ImageUrl,
            product.IsActive,
            balance?.Quantity ?? 0,
             product.Category?.Name ?? "-",
            product.CreatedAt, product.UpdatedAt
        );
    }
}