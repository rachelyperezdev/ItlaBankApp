using ItlaBankApp.Core.Application.DTOs.Email;
using ItlaBankApp.Core.Domain.Settings;

namespace ItlaBankApp.Core.Application.Interfaces.Services.Email
{
    public interface IEmailService
    {
        public MailSettings MailSettings { get; }
        Task SendAsync(EmailRequest request);
    }
}
