using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionStock.Infrastructure.Repositories;

public class StockBalanceRepository : IStockBalanceRepository
{
    private readonly AppDbContext _db;

    public StockBalanceRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<StockBalance?> GetByProductIdAsync(Guid tenantId, Guid productId, CancellationToken ct)
        => _db.StockBalances.FirstOrDefaultAsync(sb => sb.TenantId == tenantId && sb.ProductId == productId, ct);

    public Task AddAsync(StockBalance balance, CancellationToken ct)
        => _db.StockBalances.AddAsync(balance, ct).AsTask();

    public void Update(StockBalance balance)
        => _db.StockBalances.Update(balance);

    // ✅ NEW
    public Task<List<StockBalance>> GetAllAsync(Guid tenantId, CancellationToken ct)
        => _db.StockBalances
            .Where(x => x.TenantId == tenantId)
            .ToListAsync(ct);
}