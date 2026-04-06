using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;

namespace GestionStock.Application.Products.Commands.Delete;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, Result<bool>>
{
    private readonly IProductRepository _products;
    private readonly IUnitOfWork _uow;

    public DeleteProductHandler(IProductRepository products, IUnitOfWork uow)
    {
        _products = products;
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(DeleteProductCommand command, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(command.TenantId, command.Id, ct);
        if (product is null)
            throw new NotFoundException("Product not found.");

        _products.Delete(product);
        await _uow.SaveChangesAsync(ct);

        return Result<bool>.Ok(true);
    }
}