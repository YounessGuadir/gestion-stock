using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Products.Dtos
{
    public record ProductDto(
        Guid Id,
        Guid TenantId,
        Guid CategoryId,
        string Name,
        string? Description,
        decimal Price,
        string Unit,
        string? ImageUrl,
        bool IsActive,
        decimal StockQuantity,
        string CategoryName,

        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
