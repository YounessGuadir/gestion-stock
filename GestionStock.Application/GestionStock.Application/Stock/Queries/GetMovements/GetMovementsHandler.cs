using GestionStock.Application.Abstractions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Stock.DTOs;

namespace GestionStock.Application.Stock.Queries.GetMovements;

public class GetMovementsHandler : IQueryHandler<GetMovementsQuery, List<StockMovementDto>>
{
    private readonly IStockMovementRepository _movements;

    public GetMovementsHandler(IStockMovementRepository movements)
    {
        _movements = movements;
    }

    public async Task<List<StockMovementDto>> Handle(GetMovementsQuery query, CancellationToken ct)
    {
        var list = await _movements.GetByProductAsync(query.TenantId, query.ProductId, ct);

        return list.Select(m => new StockMovementDto(
            m.Id, m.TenantId, m.ProductId, m.Type, m.Quantity, m.Reason, m.CreatedBy, m.OccurredAt
        )).ToList();
    }
}