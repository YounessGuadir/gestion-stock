using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.Commands.delete
{
    public record DeleteStockMovementCommand(Guid TenantId, Guid MovementId);
}
