using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Domain.Entities;
using ItlaBankApp.Infrastructure.Persistence.Contexts;
using ItlaBankApp.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;

namespace ItlaBankApp.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly ApplicationContext _context;

        public PaymentRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<List<Payment>> GetAllAsync()
        {
            return await _context.Set<Payment>()
                            .Include(p => p.Origin)
                            .Include(p => p.Destination)
                            .Include(p => p.Beneficiary)
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }

        public async Task<int> GetAllTimeTransactions()
        {
            return await _context.Set<Payment>().CountAsync();
        }

        public async Task<int> GetDailyTransactions()
        {
            DateTime today = DateTime.Today;

            int dailyTransactionsCount = await _context.Set<Payment>()
                .Where(p => p.CreatedOn.Date == today)
                .CountAsync();

            return dailyTransactionsCount;
        }

        public async Task<int> GetAllTimePayments()
        {
            var allowedPaymentTypes = new[] { PaymentType.Express, PaymentType.CreditCard, PaymentType.Loan, PaymentType.Beneficiary };
            return await _context.Set<Payment>()
                .CountAsync(p => allowedPaymentTypes.Contains(p.PaymentType));
        }

        public async Task<int> GetDailyPayments()
        {
            DateTime today = DateTime.Today;

            var allowedPaymentTypes = new[] { PaymentType.Express, PaymentType.CreditCard, PaymentType.Loan, PaymentType.Beneficiary };
            return await _context.Set<Payment>()
                .Where(p => p.CreatedOn.Date == today && allowedPaymentTypes.Contains(p.PaymentType))
                .CountAsync();
        }
    }
}
