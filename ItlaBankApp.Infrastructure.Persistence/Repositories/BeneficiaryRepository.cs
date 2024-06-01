using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Domain.Entities;
using ItlaBankApp.Infrastructure.Persistence.Contexts;
using ItlaBankApp.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace ItlaBankApp.Infrastructure.Persistence.Repositories
{
    public class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly ApplicationContext _context;

        public BeneficiaryRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async override Task<List<Beneficiary>> GetAllAsync()
        {
            return await _context.Set<Beneficiary>()
                            .Include(b => b.Account)
                            .Where(e => !e.IsDeleted)
                            .ToListAsync();
        }

    }
}
