using ItlaBankApp.Core.Application.DTOs.Account.Common;

namespace ItlaBankApp.Core.Application.DTOs.Account
{
    public class AuthenticationResponse : BaseResponse
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public List<string> Roles { get; set; }
    }
}