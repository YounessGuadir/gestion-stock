using GestionStock.Application.Abstractions;
using GestionStock.Application.Dashboard.Dtos;
using GestionStock.Application.Dashboard.Queries;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IQueryHandler<GetDashboardQuery, DashboardDto> _handler;

    public DashboardController(IQueryHandler<GetDashboardQuery, DashboardDto> handler)
    {
        _handler = handler;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid tenantId, CancellationToken ct)
    {
        var result = await _handler.Handle(new GetDashboardQuery(tenantId), ct);

        return Ok(result);
    }
}