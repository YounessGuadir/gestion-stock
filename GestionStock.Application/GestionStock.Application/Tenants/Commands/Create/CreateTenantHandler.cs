using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Tenants.DTOs;
using GestionStock.Domain.Entities;

namespace GestionStock.Application.Tenants.Commands.Create;

public class CreateTenantHandler : ICommandHandler<CreateTenantCommand, Result<TenantDto>>
{
    private readonly ITenantRepository _tenants;
    private readonly IUnitOfWork _uow;

    public CreateTenantHandler(ITenantRepository tenants, IUnitOfWork uow)
    {
        _tenants = tenants;
        _uow = uow;
    }

    public async Task<Result<TenantDto>> Handle(CreateTenantCommand command, CancellationToken ct)
    {
        // Basic creation (validation later)
        var tenant = new Tenant(command.Name, command.Slug, command.Plan);

        await _tenants.AddAsync(tenant, ct);
        await _uow.SaveChangesAsync(ct);

        var dto = new TenantDto(
            tenant.Id,
            tenant.Name,
            tenant.Slug,
            tenant.Plan,
            tenant.IsActive,
            tenant.CreatedAt,
            tenant.UpdatedAt
        );

        return Result<TenantDto>.Ok(dto);
    }
}