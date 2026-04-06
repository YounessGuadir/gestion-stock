using GestionStock.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Domain.Entities
{
    public class StockBalance : AuditableEntity
    {
        public Guid TenantId { get; private set; }
        public Guid ProductId { get; private set; }

        public decimal Quantity { get; private set; }
        public decimal LowStockThreshold { get; private set; } = 0;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;


        // Navigation
        public Product Product { get; private set; } = default!;

        private StockBalance() { } // EF

        public StockBalance(Guid tenantId, Guid productId)
        {
            TenantId = tenantId;
            ProductId = productId;
            Quantity = 0;
        }

        public StockBalance(Guid tenantId, Guid productId, int v) : this(tenantId, productId)
        {
        }

        public void SetThreshold(decimal threshold)
        {
            LowStockThreshold = threshold;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Increase(decimal qty)
        {
            Quantity += qty;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Decrease(decimal qty)
        {
            if (Quantity - qty < 0)
                throw new InvalidOperationException("Stock cannot be negative.");

            Quantity -= qty;
            UpdatedAt = DateTime.UtcNow;
        }


        public void SetQuantity(decimal qty)
        {
            if (qty < 0) throw new InvalidOperationException("Quantity cannot be negative.");
            Quantity = qty;
            UpdatedAt = DateTime.UtcNow;
        }

    }
}
