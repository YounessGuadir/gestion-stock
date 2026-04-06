using GestionStock.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.Commands.create
{
    public record CreateStockMovementCommand(
     Guid TenantId,
     Guid ProductId,
     MovementType Type,
     decimal Quantity,
     string CreatedBy,
     string? Reason = null
 );
}
