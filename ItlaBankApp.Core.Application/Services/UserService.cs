using AutoMapper;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.Interfaces.Services.Account;
using ItlaBankApp.Core.Application.ViewModels.Product;
using Microsoft.AspNetCore.Http;
using ItlaBankApp.Core.Application.ViewModels.User;
using ItlaBankApp.Core.Application.DTOs.Email;
using ItlaBankApp.Core.Application.Enums;

namespace ItlaBankApp.Core.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        private readonly AuthenticationResponse userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IAccountService accountService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _accountService = accountService;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<int> GetActiveClients()
        {
            var users = await _accountService.GetAllUsersAsync();
            var activeClients = users.Where(u => u.Role.Contains(Roles.Client.ToString()) && u.IsActive);
            return activeClients.Count();
        }

        public async Task<int> GetInactiveClients()
        {
            var users = await _accountService.GetAllUsersAsync();
            var inactiveClients = users.Where(u => u.Role.Contains(Roles.Client.ToString()) && !u.IsActive);
            return inactiveClients.Count();
        }

        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            return await _accountService.GetUserByUsernameAsync(username);
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            return await _accountService.DeleteUserAsync(userId);
        }

        public async Task<SaveUserViewModel> GetUserByUsernameSaveAsync(string username)
        {
            return await _accountService.GetUserByUsernameSaveAsync(username);
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            return await _accountService.GetUserById(id);
        }


        public async Task<List<UserViewModel>> GetAll()
        {
            var users = await _accountService.GetAllUsersAsync();
            return _mapper.Map<List<UserViewModel>>(users);
        }

        public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }
        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
        public async Task<bool> UpdateUserAsync(string email, SaveUserViewModel request)
        {
            var updateUserResponse = await _accountService.UpdateProfileAsync(email, request);
            return updateUserResponse;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterUserAsync(registerRequest);
        }

        public async Task<string> ConfirmEmailAsync(string userId, string token)
        {
            return await _accountService.ConfirmAccountAsync(userId, token);
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin)
        {
            ForgotPasswordRequest forgotRequest = _mapper.Map<ForgotPasswordRequest>(vm);
            return await _accountService.ForgotPasswordAsync(forgotRequest, origin);
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm)
        {
            ResetPasswordRequest resetRequest = _mapper.Map<ResetPasswordRequest>(vm);
            return await _accountService.ResetPasswordAsync(resetRequest);
        }

    }
}
