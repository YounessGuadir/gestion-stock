using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.Infrastructure.Persistence.Configurations;

public class StockBalanceConfiguration : IEntityTypeConfiguration<StockBalance>
{
    public void Configure(EntityTypeBuilder<StockBalance> builder)
    {
        builder.ToTable("StockBalances");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity).HasPrecision(18, 2);
        builder.Property(x => x.LowStockThreshold).HasPrecision(18, 2);

        builder.HasIndex(x => new { x.TenantId, x.ProductId }).IsUnique(); // مهم: 1..1
    }
}