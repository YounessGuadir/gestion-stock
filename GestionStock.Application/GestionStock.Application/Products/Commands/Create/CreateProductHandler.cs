using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Products.Dtos;
using GestionStock.Domain.Entities;

namespace GestionStock.Application.Products.Commands.Create;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;
    private readonly IUnitOfWork _uow;

    public CreateProductHandler(IProductRepository products, IStockBalanceRepository balances, IUnitOfWork uow)
    {
        _products = products;
        _balances = balances;
        _uow = uow;
    }

    public async Task<Result<ProductDto>> Handle(CreateProductCommand command, CancellationToken ct)
    {
        var product = new Product(
            command.TenantId,
            command.CategoryId,
            command.Name,
            command.Price,
            command.Unit,
            command.Description,
            command.ImageUrl
        );

        await _products.AddAsync(product, ct);

        // Create StockBalance at 0
        var balance = new StockBalance(command.TenantId, product.Id);
        await _balances.AddAsync(balance, ct);

        await _uow.SaveChangesAsync(ct);

        var dto = new ProductDto(
            product.Id, product.TenantId, product.CategoryId,
            product.Name, product.Description,
            product.Price, product.Unit, product.ImageUrl,
            product.IsActive,
            balance.Quantity,
             product.Category?.Name ?? "-",
            product.CreatedAt, product.UpdatedAt
        );

        return Result<ProductDto>.Ok(dto);
    }
}