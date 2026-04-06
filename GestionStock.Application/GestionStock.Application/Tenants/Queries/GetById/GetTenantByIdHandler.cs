using GestionStock.Application.Abstractions;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Tenants.DTOs;

namespace GestionStock.Application.Tenants.Queries.GetById;

public class GetTenantByIdHandler : IQueryHandler<GetTenantByIdQuery, TenantDto>
{
    private readonly ITenantRepository _tenants;

    public GetTenantByIdHandler(ITenantRepository tenants)
    {
        _tenants = tenants;
    }

    public async Task<TenantDto> Handle(GetTenantByIdQuery query, CancellationToken ct)
    {
        var tenant = await _tenants.GetByIdAsync(query.Id, ct);
        if (tenant is null)
            throw new NotFoundException("Tenant not found.");

        return new TenantDto(
            tenant.Id, tenant.Name, tenant.Slug, tenant.Plan, tenant.IsActive, tenant.CreatedAt, tenant.UpdatedAt
        );
    }
}