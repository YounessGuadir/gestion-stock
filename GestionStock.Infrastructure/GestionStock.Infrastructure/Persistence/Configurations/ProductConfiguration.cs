using GestionStock.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GestionStock.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(200).IsRequired();
        builder.Property(x => x.Description).HasMaxLength(1000);
        builder.Property(x => x.Unit).HasMaxLength(50).IsRequired();
        builder.Property(x => x.ImageUrl).HasMaxLength(500);

        builder.Property(x => x.Price).HasPrecision(18, 2);

        builder.HasIndex(x => new { x.TenantId, x.CategoryId });
        builder.HasIndex(x => new { x.TenantId, x.Name });

        builder.HasOne(x => x.Tenant)
            .WithMany(t => t.Products)
            .HasForeignKey(x => x.TenantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // 1..1 Product <-> StockBalance
        builder.HasOne(x => x.StockBalance)
            .WithOne(sb => sb.Product)
            .HasForeignKey<StockBalance>(sb => sb.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}