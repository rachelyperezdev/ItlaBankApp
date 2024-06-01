using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Domain.Entities;
using ItlaBankApp.Infrastructure.Persistence.Contexts;
using ItlaBankApp.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace ItlaBankApp.Infrastructure.Persistence.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationContext _context;

        public ProductRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<List<Product>> GetAllAsync()
        {
            return await _context.Set<Product>()
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }

        public async Task<Product> GetByAccountAsync(int accountNumber)
        {
            return await _context.Set<Product>().FirstOrDefaultAsync(p => p.AccountId == accountNumber);
        }

        public override async Task UpdateAsync(Product entity, int id)
        {
            var entry = _context.Set<Product>().Find(id);
            if (entity.ModifiedBy == null || entity.CreatedBy == null)
            {
                entity.CreatedBy = entry.CreatedBy;
                entity.ModifiedBy = entry.ModifiedBy;
            }
            await base.UpdateAsync(entity, id);
        }

    }
}
