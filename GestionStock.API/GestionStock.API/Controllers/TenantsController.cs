using GestionStock.Application.Tenants.Commands.Create;
using GestionStock.Application.Tenants.Commands.Update;
using GestionStock.Application.Tenants.Queries.GetAll;
using GestionStock.Application.Tenants.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Authorize(Policy = "AdminOnly")]
[Route("api/[controller]")]
public class TenantsController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromServices] GetAllTenantsHandler handler, CancellationToken ct)
    {
        var result = await handler.Handle(new GetAllTenantsQuery(), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid id,
        [FromServices] GetTenantByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetTenantByIdQuery(id), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateTenantCommand command,
        [FromServices] CreateTenantHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(command, ct);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTenantCommand command,
        [FromServices] UpdateTenantHandler handler,
        CancellationToken ct)
    {
        var cmd = command with { Id = id };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }
}