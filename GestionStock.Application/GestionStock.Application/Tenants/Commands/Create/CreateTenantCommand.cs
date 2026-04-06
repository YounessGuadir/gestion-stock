using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Tenants.Commands.Create
{
    public record CreateTenantCommand(
     string Name,
     string Slug,
     string Plan
 );
}
