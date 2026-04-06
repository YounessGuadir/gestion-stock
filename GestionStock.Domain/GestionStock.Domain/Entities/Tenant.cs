using GestionStock.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class Tenant :AuditableEntity
    {
        public string Name { get; private set; } = default!;
        public string Slug { get; private set; } = default!;
        public string Plan { get; private set; } = "FREE";
        public bool IsActive { get; private set; } = true;

        // Navigation
        public ICollection<Category> Categories { get; private set; } = new List<Category>();
        public ICollection<Product> Products { get; private set; } = new List<Product>();

        private Tenant() { } // EF

        public Tenant(string name, string slug, string plan = "FREE")
        {
            Name = name;
            Slug = slug;
            Plan = plan;
        }


        public void Update(string name, string slug, string plan, bool isActive)
        {
            Name = name;
            Slug = slug;
            Plan = plan;
            IsActive = isActive;
            Touch();
        }
        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
