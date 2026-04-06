using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Stock.Commands.update;

namespace GestionStock.Application.Stock.Commands.Update;

public class UpdateStockMovementHandler : ICommandHandler<UpdateStockMovementCommand, Result<bool>>
{
    private readonly IStockMovementRepository _movements;
    private readonly IUnitOfWork _uow;

    public UpdateStockMovementHandler(IStockMovementRepository movements, IUnitOfWork uow)
    {
        _movements = movements;
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(UpdateStockMovementCommand command, CancellationToken ct)
    {
       
        var movement = await _movements.GetByIdAsync(command.TenantId, command.MovementId, ct);
        if (movement is null) throw new NotFoundException("Movement not found.");

        movement.UpdateReason(command.Reason);

        _movements.Update(movement);
        await _uow.SaveChangesAsync(ct);

        return Result<bool>.Ok(true);
    }
}