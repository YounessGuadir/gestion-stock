using GestionStock.Domain.Entities;

namespace GestionStock.Application.Contracts.Persistence
{
    public interface IStockBalanceRepository
    {
        Task AddAsync(StockBalance balance, CancellationToken ct);

        Task<StockBalance?> GetByProductIdAsync(Guid tenantId, Guid productId, CancellationToken ct);

        Task<List<StockBalance>> GetAllAsync(Guid tenantId, CancellationToken ct);

        void Update(StockBalance balance);
    }
}