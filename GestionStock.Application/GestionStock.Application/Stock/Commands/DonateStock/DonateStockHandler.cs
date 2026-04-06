using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Domain.Enums;

namespace GestionStock.Application.Stock.Commands.DonateStock;

public class DonateStockHandler : ICommandHandler<DonateStockCommand, Result<Guid>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;
    private readonly IStockMovementRepository _movements;
    private readonly IUnitOfWork _uow;

    public DonateStockHandler(IProductRepository products, IStockBalanceRepository balances, IStockMovementRepository movements, IUnitOfWork uow)
    {
        _products = products;
        _balances = balances;
        _movements = movements;
        _uow = uow;
    }

    public async Task<Result<Guid>> Handle(DonateStockCommand command, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(command.TenantId, command.ProductId, ct);
        if (product is null) throw new NotFoundException("Product not found.");

        var balance = await _balances.GetByProductIdAsync(command.TenantId, command.ProductId, ct);
        if (balance is null) throw new NotFoundException("Stock balance not found.");

        var movement = new StockMovement(
            command.TenantId,
            command.ProductId,
            MovementType.DONATION,
            command.Quantity,
            command.CreatedBy,
            command.Reason
        );

        await _movements.AddAsync(movement, ct);

        balance.Decrease(command.Quantity);
        _balances.Update(balance);

        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Ok(movement.Id);
    }
}