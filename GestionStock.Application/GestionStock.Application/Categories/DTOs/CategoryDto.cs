using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.DTOs
{
    public record CategoryDto(
      Guid Id,
      Guid TenantId,
      string Name,
      string? Description,
      DateTime CreatedAt,
      DateTime? UpdatedAt
  );
}
