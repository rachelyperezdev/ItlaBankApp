using ItlaBankApp.Core.Application.Interfaces.Repositories.Generic;
using ItlaBankApp.Core.Domain.Common;
using ItlaBankApp.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ItlaBankApp.Infrastructure.Persistence.Repositories.Generic
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(TEntity entity)
        {
            if (entity is AuditableBaseEntity auditableEntity)
            {
                auditableEntity.IsDeleted = true;
            }
            else
            {
                _context.Set<TEntity>().Remove(entity);
            }
            await _context.SaveChangesAsync();
        }

        public virtual async Task<List<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task UpdateAsync(TEntity entity, int id)
        {
            var entry = _context.Set<TEntity>().Find(id);
            _context.Entry(entry).CurrentValues.SetValues(entity);
            await _context.SaveChangesAsync();
        }
    }
}
