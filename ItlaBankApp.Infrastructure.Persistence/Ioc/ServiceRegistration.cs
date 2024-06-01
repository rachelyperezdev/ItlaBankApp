using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Application.Interfaces.Repositories.Generic;
using ItlaBankApp.Core.Domain.Common;
using ItlaBankApp.Infrastructure.Persistence.Contexts;
using ItlaBankApp.Infrastructure.Persistence.Interceptor;
using ItlaBankApp.Infrastructure.Persistence.Repositories;
using ItlaBankApp.Infrastructure.Persistence.Repositories.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaBankApp.Infrastructure.Persistence.Ioc
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<AuditableInterceptor>();

            #region Context
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options =>
                                   options.UseInMemoryDatabase(databaseName: "NetBankingDb"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>((sp, options) =>
                {
                    var interceptor = sp.GetService<AuditableInterceptor>();
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                        b => b.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName))
                                        .AddInterceptors(interceptor);
                });
            }
            #endregion

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            #endregion
        }
    }
}
