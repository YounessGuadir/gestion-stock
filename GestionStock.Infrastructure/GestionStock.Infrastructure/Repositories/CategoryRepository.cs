using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GestionStock.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _db;

    public CategoryRepository(AppDbContext db)
    {
        _db = db;
    }

    public Task<Category?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken ct)
        => _db.Categories.FirstOrDefaultAsync(c => c.TenantId == tenantId && c.Id == id, ct);

    public Task<List<Category>> GetAllAsync(Guid tenantId, CancellationToken ct)
        => _db.Categories
            .Where(c => c.TenantId == tenantId)
            .OrderByDescending(c => c.CreatedAt)
            .ToListAsync(ct);

    public Task AddAsync(Category category, CancellationToken ct)
        => _db.Categories.AddAsync(category, ct).AsTask();

    public void Update(Category category)
        => _db.Categories.Update(category);

    public void Delete(Category category)
        => _db.Categories.Remove(category);
}