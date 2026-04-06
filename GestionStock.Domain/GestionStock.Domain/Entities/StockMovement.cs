using GestionStock.Domain.Common;
using GestionStock.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class StockMovement : AuditableEntity
    {
        public Guid TenantId { get; private set; }
        public Guid ProductId { get; private set; }

        public MovementType Type { get; private set; }

        public decimal Quantity { get; private set; }
        public string? Reason { get; private set; }
        public string CreatedBy { get; private set; } = default!;
        public DateTime OccurredAt { get; private set; } = DateTime.UtcNow;

        // Navigation
        public Product Product { get; private set; } = default!;


        private StockMovement() { } // For EF Core  


        public StockMovement(Guid tenantId, Guid productId, MovementType type, decimal quantity, string createdBy, string? reason = null, DateTime? occurredAt = null)
        {
            TenantId = tenantId;
            ProductId = productId;
            Type = type;
            Quantity = quantity;
            CreatedBy = createdBy;
            Reason = reason;
            OccurredAt = occurredAt ?? DateTime.UtcNow;
        }


        public void UpdateReason(string? reason) => Reason = reason;






    }
}
