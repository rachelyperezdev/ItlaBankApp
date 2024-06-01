using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Domain.Common;

namespace ItlaBankApp.Infrastructure.Persistence.Interceptor
{
    public sealed class AuditableInterceptor : SaveChangesInterceptor
    {
        public readonly IHttpContextAccessor _httpContextAccessor;
        public AuthenticationResponse _user;

        public AuditableInterceptor(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            DbContext? dbContext = eventData.Context;
            if (dbContext is null)
            {
                return base.SavingChangesAsync(eventData, result, cancellationToken);
            }

            IEnumerable<EntityEntry<AuditableBaseEntity>> entries = dbContext.ChangeTracker.Entries<AuditableBaseEntity>();

            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            foreach (EntityEntry<AuditableBaseEntity> entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = _user.UserName;
                    entry.Entity.CreatedOn = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified && entry.Entity.IsDeleted)
                {
                    entry.Entity.DeletedBy = _user.UserName;
                    entry.Entity.DeletedOn = DateTime.Now;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.ModifiedBy = _user.UserName;
                    entry.Entity.ModifiedOn = DateTime.Now;
                }
            }

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
