using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Tenants.DTOs
{
    public record TenantDto(
    Guid Id,
    string Name,
    string Slug,
    string Plan,
    bool IsActive,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
}
