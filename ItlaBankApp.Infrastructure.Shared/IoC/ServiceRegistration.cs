using ItlaBankApp.Core.Application.Interfaces.Services.Email;
using ItlaBankApp.Core.Domain.Settings;
using ItlaBankApp.Infrastructure.Shared.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ItlaBankApp.Infrastructure.Shared.IoC
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration _config)
        {
            services.Configure<MailSettings>(_config.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
        }
    }
}