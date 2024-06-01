using ItlaBankApp.Core.Application.Interfaces.Repositories.Generic;
using ItlaBankApp.Core.Domain.Entities;

namespace ItlaBankApp.Core.Application.Interfaces.Repositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<Product> GetByAccountAsync(int accountNumber);
    }
}
