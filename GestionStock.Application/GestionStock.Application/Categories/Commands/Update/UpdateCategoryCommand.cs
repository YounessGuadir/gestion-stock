using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Categories.Commands.Update
{
    public record UpdateCategoryCommand(Guid TenantId, Guid Id, string Name, string? Description);
}
