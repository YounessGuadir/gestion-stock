using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionStock.Infrastructure.Repositories;

public class StockMovementRepository : IStockMovementRepository
{
    private readonly AppDbContext _db;

    public StockMovementRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task AddAsync(StockMovement movement, CancellationToken ct)
        => _db.StockMovements.AddAsync(movement, ct).AsTask();

    public Task<StockMovement?> GetByIdAsync(Guid tenantId, Guid movementId, CancellationToken ct)
        => _db.StockMovements.FirstOrDefaultAsync(m => m.TenantId == tenantId && m.Id == movementId, ct);

    public Task<List<StockMovement>> GetByProductAsync(Guid tenantId, Guid productId, CancellationToken ct)
        => _db.StockMovements
            .Where(m => m.TenantId == tenantId && m.ProductId == productId)
            .OrderByDescending(m => m.OccurredAt)
            .ToListAsync(ct);

    public void Update(StockMovement movement)
        => _db.StockMovements.Update(movement);

    public void Delete(StockMovement movement)
        => _db.StockMovements.Remove(movement);

    // ✅ NEW
    public Task<List<StockMovement>> GetAllAsync(Guid tenantId, CancellationToken ct)
        => _db.StockMovements
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(ct);
}