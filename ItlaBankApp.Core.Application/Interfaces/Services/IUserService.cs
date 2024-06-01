using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;

namespace ItlaBankApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<string> ConfirmEmailAsync(string userId, string token);
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin);
        Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordViewModel vm);
        Task SignOutAsync();
        Task<UserViewModel> GetUserById(string id);
        Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordViewModel vm, string origin);
        Task<bool> UpdateUserAsync(string email, SaveUserViewModel request);
        Task<UserViewModel> GetUserByUsernameAsync(string username);
        Task<SaveUserViewModel> GetUserByUsernameSaveAsync(string username);
        Task<List<UserViewModel>> GetAll();
        Task<bool> DeleteUserAsync(string userId);
        Task<int> GetActiveClients();
        Task<int> GetInactiveClients();
    }
}
 