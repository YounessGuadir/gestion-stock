using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Commands.Delete
{
    public record DeleteCategoryCommand(Guid TenantId, Guid Id);
}
