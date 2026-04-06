using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.Infrastructure.Persistence.Configurations;

public class StockMovementConfiguration : IEntityTypeConfiguration<StockMovement>
{
    public void Configure(EntityTypeBuilder<StockMovement> builder)
    {
        builder.ToTable("StockMovements");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Quantity).HasPrecision(18, 2);
        builder.Property(x => x.Reason).HasMaxLength(500);
        builder.Property(x => x.CreatedBy).HasMaxLength(200).IsRequired();

        builder.HasIndex(x => new { x.TenantId, x.ProductId, x.OccurredAt });

        builder.HasOne(x => x.Product)
            .WithMany(p => p.Movements)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}