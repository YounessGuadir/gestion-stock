using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Stock.Commands.delete;

namespace GestionStock.Application.Stock.Commands.Delete;

public class DeleteStockMovementHandler : ICommandHandler<DeleteStockMovementCommand, Result<bool>>
{
    private readonly IStockMovementRepository _movements;
    private readonly IUnitOfWork _uow;

    public DeleteStockMovementHandler(IStockMovementRepository movements, IUnitOfWork uow)
    {
        _movements = movements;
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(DeleteStockMovementCommand command, CancellationToken ct)
    {
        var movement = await _movements.GetByIdAsync(command.TenantId, command.MovementId, ct);
        if (movement is null) throw new NotFoundException("Movement not found.");

        _movements.Delete(movement);
        await _uow.SaveChangesAsync(ct);

        return Result<bool>.Ok(true);
    }
}