using GestionStock.Domain.Entities;

namespace GestionStock.Application.Contracts.Persistence;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<List<Tenant>> GetAllAsync(CancellationToken ct);

    Task AddAsync(Tenant tenant, CancellationToken ct);
    void Update(Tenant tenant);
}