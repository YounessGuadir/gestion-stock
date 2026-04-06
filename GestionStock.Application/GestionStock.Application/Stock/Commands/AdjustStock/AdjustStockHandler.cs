using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Domain.Enums;

namespace GestionStock.Application.Stock.Commands.AdjustStock;

public class AdjustStockHandler : ICommandHandler<AdjustStockCommand, Result<Guid>>
{
    private readonly IProductRepository _products;
    private readonly IStockBalanceRepository _balances;
    private readonly IStockMovementRepository _movements;
    private readonly IUnitOfWork _uow;

    public AdjustStockHandler(
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

    public async Task<Result<Guid>> Handle(AdjustStockCommand command, CancellationToken ct)
    {
        // 1️⃣ Check product exists
        var product = await _products.GetByIdAsync(
            command.TenantId,
            command.ProductId,
            ct);

        if (product is null)
            throw new NotFoundException("Product not found.");

        // 2️⃣ Get balance
        var balance = await _balances.GetByProductIdAsync(
            command.TenantId,
            command.ProductId,
            ct);

        // 3️⃣ If balance does not exist → create it
        if (balance is null)
        {
            balance = new StockBalance(
                command.TenantId,
                command.ProductId,
                0 // initial quantity
            );

            await _balances.AddAsync(balance, ct);
        }

        // 4️⃣ Add quantity cumulatively
        var newQuantity = balance.Quantity + command.Quantity;

        balance.SetQuantity(newQuantity);
        _balances.Update(balance);

        // 5️⃣ Create movement history
        var movement = new StockMovement(
            command.TenantId,
            command.ProductId,
            MovementType.ADJUSTMENT,
            command.Quantity,
            command.CreatedBy,
            command.Reason
        );

        await _movements.AddAsync(movement, ct);

        // 6️⃣ Save changes
        await _uow.SaveChangesAsync(ct);

        return Result<Guid>.Ok(movement.Id);
    }
}