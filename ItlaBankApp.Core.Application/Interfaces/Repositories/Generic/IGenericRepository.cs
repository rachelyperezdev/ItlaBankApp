namespace ItlaBankApp.Core.Application.Interfaces.Repositories.Generic
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity, int id);
        Task DeleteAsync(TEntity entity);
    }
}
