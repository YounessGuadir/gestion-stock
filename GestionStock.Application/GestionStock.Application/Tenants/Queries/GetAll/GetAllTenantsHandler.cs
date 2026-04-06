using GestionStock.Application.Abstractions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Tenants.DTOs;

namespace GestionStock.Application.Tenants.Queries.GetAll;

public class GetAllTenantsHandler : IQueryHandler<GetAllTenantsQuery, List<TenantDto>>
{
    private readonly ITenantRepository _tenants;

    public GetAllTenantsHandler(ITenantRepository tenants)
    {
        _tenants = tenants;
    }

    public async Task<List<TenantDto>> Handle(GetAllTenantsQuery query, CancellationToken ct)
    {
        var list = await _tenants.GetAllAsync(ct);

        return list.Select(t => new TenantDto(
            t.Id, t.Name, t.Slug, t.Plan, t.IsActive, t.CreatedAt, t.UpdatedAt
        )).ToList();
    }
}