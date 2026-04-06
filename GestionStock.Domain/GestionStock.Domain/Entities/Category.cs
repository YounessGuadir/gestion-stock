using GestionStock.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class Category :AuditableEntity
    {
        public Guid TenantId { get; private set; }
        public string Name { get; private set; } = default!;
        public string? Description { get; private set; }

        // Navigation
        public Tenant Tenant { get; private set; } = default!;
        public ICollection<Product> Products { get; private set; } = new List<Product>();


        public Category() { }

        public Category(Guid tenantId, string name, string? description = null)
        {
            TenantId = tenantId;
            Name = name;
            Description = description;
        }
        public void Update(string name, string? description)
        {
            Name = name;
            Description = description;
            Touch();
        }
    }
}
