using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Commands.Create
{
    public record CreateCategoryCommand(Guid TenantId, string Name, string? Description);
   
}
