using GestionStock.Application.Abstractions;
using GestionStock.Application.Common;
using GestionStock.Application.Common.Exceptions;
using GestionStock.Application.Contracts.Persistence;
using GestionStock.Application.Tenants.DTOs;

namespace GestionStock.Application.Tenants.Commands.Update;

public class UpdateTenantHandler : ICommandHandler<UpdateTenantCommand, Result<TenantDto>>
{
    private readonly ITenantRepository _tenants;
    private readonly IUnitOfWork _uow;

    public UpdateTenantHandler(ITenantRepository tenants, IUnitOfWork uow)
    {
        _tenants = tenants;
        _uow = uow;
    }

    public async Task<Result<TenantDto>> Handle(UpdateTenantCommand command, CancellationToken ct)
    {
        var tenant = await _tenants.GetByIdAsync(command.Id, ct);
        if (tenant is null)
            throw new NotFoundException("Tenant not found.");

        // depends on your Tenant entity methods:
        // easiest: add an Update(...) method in Tenant entity
        // For now, if properties are private set, add a method in Tenant:
        tenant.Update(command.Name, command.Slug, command.Plan, command.IsActive);

        _tenants.Update(tenant);
        await _uow.SaveChangesAsync(ct);

        var dto = new TenantDto(
            tenant.Id, tenant.Name, tenant.Slug, tenant.Plan, tenant.IsActive, tenant.CreatedAt, tenant.UpdatedAt
        );

        return Result<TenantDto>.Ok(dto);
    }
}