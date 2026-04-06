using GestionStock.Application.Stock.Commands.AdjustStock;
using GestionStock.Application.Stock.Commands.DonateStock;
using GestionStock.Application.Stock.Queries.GetBalance;
using GestionStock.Application.Stock.Queries.GetMovements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/products/{productId:guid}/stock")]
public class StockController : ControllerBase
{
    [HttpGet("balance")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetBalance(
        Guid tenantId,
        Guid productId,
        [FromServices] GetBalanceHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetBalanceQuery(tenantId, productId), ct);
        return Ok(result);
    }

    [HttpGet("movements")]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetMovements(
        Guid tenantId,
        Guid productId,
        [FromServices] GetMovementsHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetMovementsQuery(tenantId, productId), ct);
        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPost("donate")]
    public async Task<IActionResult> Donate(
        Guid tenantId,
        Guid productId,
        [FromBody] DonateStockCommand command,
        [FromServices] DonateStockHandler handler,
        CancellationToken ct)
    {
        var cmd = command with { TenantId = tenantId, ProductId = productId };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }

    [HttpPost("adjust")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> Adjust(
        Guid tenantId,
        Guid productId,
        [FromBody] AdjustStockCommand command,
        [FromServices] AdjustStockHandler handler,
        CancellationToken ct)
    {
        var cmd = command with { TenantId = tenantId, ProductId = productId };
        var result = await handler.Handle(cmd, ct);
        return Ok(result);
    }
}