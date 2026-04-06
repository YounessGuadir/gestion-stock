using GestionStock.Domain.Entities;

namespace GestionStock.Application.Contracts.Persistence
{
    public interface IStockMovementRepository
    {
        Task AddAsync(StockMovement movement, CancellationToken ct);

        Task<StockMovement?> GetByIdAsync(Guid tenantId, Guid movementId, CancellationToken ct);

        Task<List<StockMovement>> GetByProductAsync(Guid tenantId, Guid productId, CancellationToken ct);

        Task<List<StockMovement>> GetAllAsync(Guid tenantId, CancellationToken ct);
        void Update(StockMovement movement);

        void Delete(StockMovement movement); // اختياري (إلا عندك delete)
    }
}