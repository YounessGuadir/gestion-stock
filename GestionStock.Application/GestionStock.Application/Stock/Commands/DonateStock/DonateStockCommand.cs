using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.Commands.DonateStock
{
    public record DonateStockCommand(
     Guid TenantId,
     Guid ProductId,
     decimal Quantity,
     string CreatedBy,
     string? Reason = null
 );
}
