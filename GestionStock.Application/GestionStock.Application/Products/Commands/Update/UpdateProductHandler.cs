using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Products.Dtos;

namespace GestionStock.Application.Products.Commands.Update;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, Result<ProductDto>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;
    private readonly IUnitOfWork _uow;

    public UpdateProductHandler(IProductRepository products, IStockBalanceRepository balances, IUnitOfWork uow)
    {
        _products = products;
        _balances = balances;
        _uow = uow;
    }

    public async Task<Result<ProductDto>> Handle(UpdateProductCommand command, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(command.TenantId, command.Id, ct);
        if (product is null)
            throw new NotFoundException("Product not found.");

        product.Update(
            command.Name,
            command.Price,
            command.Unit,
            command.CategoryId,
            command.Description,
            command.ImageUrl,
            command.IsActive
        );

        _products.Update(product);

        var balance = await _balances.GetByProductIdAsync(command.TenantId, product.Id, ct);

        await _uow.SaveChangesAsync(ct);

        var dto = new ProductDto(
            product.Id, product.TenantId, product.CategoryId,
            product.Name, product.Description,
            product.Price, product.Unit, product.ImageUrl,
            product.IsActive,
            balance?.Quantity ?? 0,
            product.Category?.Name ?? "-",
            product.CreatedAt, product.UpdatedAt
        );

        return Result<ProductDto>.Ok(dto);
    }
}