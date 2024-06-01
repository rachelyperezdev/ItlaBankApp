using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using ItlaBankApp.Infrastructure.Identity.Contexts;
using ItlaBankApp.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Core.Application.Interfaces.Services.Account;

namespace ItlaBankApp.Infrastructure.Identity.IoC
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if(configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDb"));
            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                        m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });
            }
            #endregion

            #region Identity
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User"; 
                options.AccessDeniedPath = "/User/AccessDenied"; 
            });

            services.AddAuthentication();
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<SignInManager<ApplicationUser>>();
            #endregion
        }
    }
}
