using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Products.Queries.GetAll
{
    public record GetProductsQuery(Guid TenantId, Guid? CategoryId = null, bool? IsActive = null);
}
