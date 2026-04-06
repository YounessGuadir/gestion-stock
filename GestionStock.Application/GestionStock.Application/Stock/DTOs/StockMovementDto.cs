using GestionStock.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.DTOs
{
    public record StockMovementDto(
      Guid Id,
      Guid TenantId,
      Guid ProductId,
      MovementType Type,
      decimal Quantity,
      string? Reason,
      string CreatedBy,
      DateTime OccurredAt
  );
}
