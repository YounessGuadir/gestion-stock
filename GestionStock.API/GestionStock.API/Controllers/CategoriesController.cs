using GestionStock.Application.Categories.Commands.Create;
using GestionStock.Application.Categories.Commands.Delete;
using GestionStock.Application.Categories.Commands.Update;
using GestionStock.Application.Categories.Queries.GetAll;
using GestionStock.Application.Categories.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/categories")]
public class CategoriesController : ControllerBase
{
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet]
    public async Task<IActionResult> GetAll(
        Guid tenantId,
        [FromServices] GetCategoriesHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetCategoriesQuery(tenantId), ct);
        return Ok(result);
    }
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid tenantId,
        Guid id,
        [FromServices] GetCategoryByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetCategoryByIdQuery(tenantId, id), ct);
        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(
        Guid tenantId,
        [FromBody] CreateCategoryCommand command,
        [FromServices] CreateCategoryHandler handler,
        CancellationToken ct)
    {
        var cmd = command with { TenantId = tenantId };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid tenantId,
        Guid id,
        [FromBody] UpdateCategoryCommand command,
        [FromServices] UpdateCategoryHandler handler,
        CancellationToken ct)
    {
        var cmd = command with { TenantId = tenantId, Id = id };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid tenantId,
        Guid id,
        [FromServices] DeleteCategoryHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new DeleteCategoryCommand(tenantId, id), ct);
        return Ok(result);
    }
}