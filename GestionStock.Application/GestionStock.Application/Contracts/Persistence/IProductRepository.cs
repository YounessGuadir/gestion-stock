using GestionStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Contracts.Persistence
{
    public interface IProductRepository
    {
        Task<Product?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken ct);

        Task<List<Product>> GetAllAsync(Guid tenantId, Guid? categoryId, bool? isActive, CancellationToken ct);

        Task AddAync(Product product, CancellationToken ct);

        void Update(Product product);
        void Delete(Product product);
        Task AddAsync(Product product, CancellationToken ct);
    }
}
