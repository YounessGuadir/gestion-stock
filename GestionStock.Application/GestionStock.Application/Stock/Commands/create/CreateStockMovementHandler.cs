using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Stock.Commands.create;
using GestionStock.Domain.Entities;
using GestionStock.Domain.Enums;

namespace GestionStock.Application.Stock.Commands.Create;

public class CreateStockMovementHandler : ICommandHandler<CreateStockMovementCommand, Result<Guid>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;
    private readonly IStockMovementRepository _movements;
    private readonly IUnitOfWork _uow;

    public CreateStockMovementHandler(
        IProductRepository products,
        IStockBalanceRepository balances,
        IStockMovementRepository movements,
        IUnitOfWork uow)
    {
        _products = products;
        _balances = balances;
        _movements = movements;
        _uow = uow;
    }

    public async Task<Result<Guid>> Handle(CreateStockMovementCommand command, CancellationToken ct)
    {
        var product = await _products.GetByIdAsync(command.TenantId, command.ProductId, ct);
        if (product is null) throw new NotFoundException("Product not found.");

        var balance = await _balances.GetByProductIdAsync(command.TenantId, command.ProductId, ct);
        if (balance is null) throw new NotFoundException("Stock balance not found.");

        // 1) Create movement
        var movement = new StockMovement(
            command.TenantId,
            command.ProductId,
            command.Type,
            command.Quantity,
            command.CreatedBy,
            command.Reason
        );

        await _movements.AddAsync(movement, ct);

        // 2) Update balance based on type
        switch (command.Type)
        {
            case MovementType.IN:
            case MovementType.ADJUSTMENT:
                balance.Increase(command.Quantity);
                break;

            case MovementType.OUT:
            case MovementType.DONATION:
                balance.Decrease(command.Quantity);
                break;

            default:
                throw new InvalidOperationException("Unsupported movement type.");
        }

        _balances.Update(balance);

        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Ok(movement.Id);
    }
}