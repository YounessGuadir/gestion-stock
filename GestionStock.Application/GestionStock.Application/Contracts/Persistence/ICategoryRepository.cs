using GestionStock.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Application.Contracts.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(Guid tenantId, Guid id, CancellationToken ct);
        Task<List<Category>> GetAllAsync(Guid tenantId, CancellationToken ct);

        Task AddAsync(Category category, CancellationToken ct);

        void Update(Category category);
        void Delete(Category category);
    }
}
