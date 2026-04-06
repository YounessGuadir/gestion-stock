using GestionStock.Application.Common.Interfaces;
using GestionStock.Application.Products.Commands.Create;
using GestionStock.Application.Products.Commands.Delete;
using GestionStock.Application.Products.Commands.Update;
using GestionStock.Application.Products.Queries.GetAll;
using GestionStock.Application.Products.Queries.GetById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GestionStock.API.Controllers;

[ApiController]
[Route("api/tenants/{tenantId:guid}/products")]
public class ProductsController : ControllerBase
{
    [HttpGet]
    [Authorize(Policy = "UserOrAdmin")]
    public async Task<IActionResult> GetAll(
        Guid tenantId,
        [FromQuery] Guid? categoryId,
        [FromQuery] bool? isActive,
        [FromServices] GetProductsHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetProductsQuery(tenantId, categoryId, isActive), ct);
        return Ok(result);
    }
    [Authorize(Policy = "UserOrAdmin")]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(
        Guid tenantId,
        Guid id,
        [FromServices] GetProductByIdHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new GetProductByIdQuery(tenantId, id), ct);
        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    public async Task<IActionResult> Create(
    Guid tenantId,
    [FromForm] Guid categoryId,
    [FromForm] string name,
    [FromForm] string? description,
    [FromForm] decimal price,
    [FromForm] string unit,
    IFormFile? image,
    [FromServices] CreateProductHandler handler,
    [FromServices] IFileStorageService fileStorage,
    CancellationToken ct)
    {
        string? imageUrl = null;

        if (image != null)
        {
            using var stream = image.OpenReadStream();

            imageUrl = await fileStorage.SaveProductImageAsync(
                stream,
                image.FileName,
                ct
            );
        }
        var command = new CreateProductCommand(
            tenantId,
            categoryId,
            name,
            description,
            price,
            unit,
            imageUrl
        );

        var result = await handler.Handle(command, ct);

        return Ok(result);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
    Guid tenantId,
    Guid id,
    [FromForm] Guid categoryId,
    [FromForm] string name,
    [FromForm] string? description,
    [FromForm] decimal price,
    [FromForm] string unit,
    [FromForm] bool isActive,
    IFormFile? image,
    [FromServices] UpdateProductHandler handler,
    [FromServices] IFileStorageService fileStorage,
    CancellationToken ct)
    {
        string? imageUrl = null;

        if (image != null)
        {
            using var stream = image.OpenReadStream();

            imageUrl = await fileStorage.SaveProductImageAsync(
                stream,
                image.FileName,
                ct
            );
        }

        var command = new UpdateProductCommand(
            tenantId,
            id,
            categoryId,
            name,
            description,
            price,
            unit,
            imageUrl,
            isActive
        );

        var result = await handler.Handle(command, ct);

        return Ok(result);
    }
    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(
        Guid tenantId,
        Guid id,
        [FromServices] DeleteProductHandler handler,
        CancellationToken ct)
    {
        var result = await handler.Handle(new DeleteProductCommand(tenantId, id), ct);
        return Ok(result);
    }
}