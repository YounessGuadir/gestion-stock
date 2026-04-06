using GestionStock.Application.Abstractions;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Stock.DTOs;

namespace GestionStock.Application.Stock.Queries.GetBalance;

public class GetBalanceHandler : IQueryHandler<GetBalanceQuery, StockBalanceDto>
{
    private readonly IStockBalanceRepository _balances;

    public GetBalanceHandler(IStockBalanceRepository balances)
    {
        _balances = balances;
    }

    public async Task<StockBalanceDto> Handle(GetBalanceQuery query, CancellationToken ct)
    {
        var balance = await _balances.GetByProductIdAsync(query.TenantId, query.ProductId, ct);
        if (balance is null) throw new NotFoundException("Stock balance not found.");

        return new StockBalanceDto(
            balance.Id,
            balance.TenantId,
            balance.ProductId,
            balance.Quantity,
            balance.LowStockThreshold,
            balance.UpdatedAt
        );
    }
}