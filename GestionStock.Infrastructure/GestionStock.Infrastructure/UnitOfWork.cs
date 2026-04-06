using GestionStock.Application.Abstractions;
using GestionStock.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionStock.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;


        public UnitOfWork(AppDbContext db)
        {
            _db = db;
        }
        public Task<int> SaveChangesAsync(CancellationToken ct)
      => _db.SaveChangesAsync(ct);
    }

}
