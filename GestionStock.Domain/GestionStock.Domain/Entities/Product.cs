using GestionStock.Domain.Common;

namespace GestionStock.Domain.Entities;

public class Product : AuditableEntity
{
    public Guid TenantId { get; private set; }
    public Guid CategoryId { get; private set; }

    public string Name { get; private set; } = default!;
    public string? Description { get; private set; }
    public decimal Price { get; private set; }
    public string Unit { get; private set; } = "pcs";
    public string? ImageUrl { get; private set; }
    public bool IsActive { get; private set; } = true;



    // Navigation
    public Tenant Tenant { get; private set; } = default!;
    public Category Category { get; private set; } = default!;
    public StockBalance StockBalance { get; private set; } = default!;
    public ICollection<StockMovement> Movements { get; private set; } = new List<StockMovement>();

    private Product() { } // For EF Core

    public Product(Guid tenantId, Guid categoryId, string name, decimal price, string unit = "pcs", string? description = null, string? ImagePath = null)
    {
        TenantId = tenantId;
        CategoryId = categoryId;
        Name = name;
        Price = price;
        Unit = unit;
        Description = description;
        ImageUrl = ImagePath;
    }
    public void Update(string name, decimal price, string unit, Guid categoryId, string? description, string? imageUrl, bool isActive)
    {
        Name = name;
        Price = price;
        Unit = unit;
        CategoryId = categoryId;
        Description = description;
        ImageUrl = imageUrl;
        IsActive = isActive;
        Touch();
    }




}
