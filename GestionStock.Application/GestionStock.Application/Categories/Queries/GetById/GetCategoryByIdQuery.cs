using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Queries.GetById
{
    public record GetCategoryByIdQuery(Guid TenantId, Guid Id);
}
