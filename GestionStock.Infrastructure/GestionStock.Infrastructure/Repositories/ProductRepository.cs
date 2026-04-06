using GestionStock.Application.Contracts.Persistence;
using GestionStock.Domain.Entities;
using GestionStock.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Infrastructure.Repositories
{
    public class ProductRepository :IProductRepository
    {
        private readonly AppDbContext _db;

        public ProductRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<Product?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken ct)
        => _db.Products.FirstOrDefaultAsync(p => p.TenantId == tenantId && p.Id == id, ct);

        public Task<List<Product>> GetAllAsync(Guid tenantId, Guid? categoryId, bool? isActive, CancellationToken ct)
        {
            IQueryable<Product> q = _db.Products
                .Include(p => p.Category) // 🔥 هذا هو الحل
                .Where(p => p.TenantId == tenantId);

            if (categoryId.HasValue)
                q = q.Where(p => p.CategoryId == categoryId.Value);

            if (isActive.HasValue)
                q = q.Where(p => p.IsActive == isActive.Value);

            return q.OrderByDescending(p => p.CreatedAt).ToListAsync(ct);
        }

        public Task<bool> ExistsAsync(Guid tenantId, Guid id, CancellationToken ct)
            => _db.Products.AnyAsync(p => p.TenantId == tenantId && p.Id == id, ct);

        public Task AddAsync(Product product, CancellationToken ct)
            => _db.Products.AddAsync(product, ct).AsTask();

        public void Update(Product product)
            => _db.Products.Update(product);

        public void Delete(Product product)
            => _db.Products.Remove(product);

        public Task AddAync(Product product, CancellationToken ct)
          => _db.Products.AddAsync(product, ct).AsTask();
    }

}
