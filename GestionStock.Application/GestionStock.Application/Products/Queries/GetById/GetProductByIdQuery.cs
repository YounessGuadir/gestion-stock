using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Products.Queries.GetById
{
    public record GetProductByIdQuery(Guid TenantId, Guid Id);
}
