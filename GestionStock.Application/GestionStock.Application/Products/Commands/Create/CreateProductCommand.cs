using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Products.Commands.Create
{
    public record CreateProductCommand(
    Guid TenantId,
    Guid CategoryId,
    string Name,
    string? Description,
    decimal Price,
    string Unit,
    string? ImageUrl
);
}
