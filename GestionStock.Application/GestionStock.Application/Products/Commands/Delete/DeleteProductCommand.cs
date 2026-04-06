using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Products.Commands.Delete
{
    public record DeleteProductCommand(Guid TenantId, Guid Id);
}
