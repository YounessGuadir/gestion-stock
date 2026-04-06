using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Tenants.Commands.Update
{
    public record UpdateTenantCommand(
     Guid Id,
     string Name,
     string Slug,
     string Plan,
     bool IsActive
 );
}
