using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Stock.Queries.GetMovements
{
    public record GetMovementsQuery(Guid TenantId, Guid ProductId);
}
