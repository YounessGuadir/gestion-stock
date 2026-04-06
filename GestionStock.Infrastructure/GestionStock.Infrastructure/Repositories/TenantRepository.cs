using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionStock.Infrastructure.Repositories;

public class TenantRepository : ITenantRepository
{
    private readonly AppDbContext _db;

    public TenantRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct)
        => _db.Tenants.FirstOrDefaultAsync(t => t.Id == id, ct);

    public Task<List<Tenant>> GetAllAsync(CancellationToken ct)
        => _db.Tenants.OrderByDescending(t => t.CreatedAt).ToListAsync(ct);

    public Task AddAsync(Tenant tenant, CancellationToken ct)
        => _db.Tenants.AddAsync(tenant, ct).AsTask();

    public void Update(Tenant tenant)
        => _db.Tenants.Update(tenant);
}