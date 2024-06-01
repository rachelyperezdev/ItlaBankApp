using ItlaBankApp.Infrastructure.Identity.Entities;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace ItlaBankApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserDTO?>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDTO?> GetUserByUserNameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {
            throw new NotImplementedException();
        }

        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
