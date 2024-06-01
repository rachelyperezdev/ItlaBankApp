using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;

namespace ItlaBankApp.Core.Application.Interfaces.Services.Account
{
    public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        Task<List<UserDTO>> GetAllUsersAsync();
        Task<RegisterResponse> RegisterUserAsync(RegisterRequest request);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task SignOutAsync();
        Task ToogleUserActiveStatusAsync(string id);
        Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<UserViewModel> GetUserById(string id);
        Task<SaveUserViewModel> GetUserByUsernameSaveAsync(string username);
        Task<bool> UpdateProfileAsync(string email, SaveUserViewModel profile);
        Task<bool> DeleteUserAsync(string userId);
    }
}