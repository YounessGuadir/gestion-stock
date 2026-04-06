using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.Commands.update
{
    public record UpdateStockMovementCommand(
       Guid TenantId,
       Guid MovementId,
       string? Reason
   );
}
