using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.Interfaces.Services.Generic;
using ItlaBankApp.Core.Application.Services;
using ItlaBankApp.Core.Application.Services.Generic;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ItlaBankApp.Core.Application.IoC
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            services.AddTransient(typeof(IGenericService<, ,>), typeof(GenericService<, ,>));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IBeneficiaryService, BeneficiaryService>();
            services.AddTransient<IPaymentService, PaymentService>();
            #endregion
        }
    }
}
