using ItlaBankApp.Core.Application.Interfaces.Repositories.Generic;
using ItlaBankApp.Core.Domain.Entities;

namespace ItlaBankApp.Core.Application.Interfaces.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<int> GetDailyPayments();
        Task<int> GetAllTimePayments();
        Task<int> GetDailyTransactions();
        Task<int> GetAllTimeTransactions();
    }
}
