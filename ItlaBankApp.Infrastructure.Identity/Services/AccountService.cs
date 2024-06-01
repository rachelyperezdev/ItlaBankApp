using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Enums;
using ItlaBankApp.Core.Application.Interfaces.Services.Account;
using ItlaBankApp.Core.Application.Interfaces.Services.Email;
using ItlaBankApp.Core.Application.ViewModels.User;
using ItlaBankApp.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace ItlaBankApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByNameAsync(userId);
            if (user == null)
            {
                // Si el usuario no se encuentra
                return false;
            }

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                // Si ocurre algún error al eliminar el usuario
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateProfileAsync(string email, SaveUserViewModel profile)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                throw new ApplicationException($"No se pudo encontrar el usuario con ID {email}.");
            }

            user.UserName = profile.Username;
            user.Email = profile.Email;
            user.FirstName = profile.FirstName;
            user.LastName = profile.LastName;
            user.PhoneNumber = profile.PhoneNumber;
            user.IdentificationCard  = profile.IdentificationCard;
            user.IsActive = profile.IsActive;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return false;
            }

            return true;
        }
        public async Task<UserViewModel> GetUserByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }

            var userViewModel = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Username = user.UserName,
            };

            return userViewModel;
        }

        public async Task<UserViewModel> GetUserById(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var userViewModel = new UserViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Username = user.UserName,

            };

            return userViewModel;
        }
        public async Task<SaveUserViewModel> GetUserByUsernameSaveAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return null;
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userViewModel = new SaveUserViewModel
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                IdentificationCard = user.IdentificationCard,
                Username = user.UserName,
                IsActive = user.IsActive,
                Role = roles.FirstOrDefault(),
            };

            return userViewModel;
        }

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred wgile confirming {user.Email}.";
            }
        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No hay cuentas registradas con '{request.UserName}'";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, false);

            if (!user.IsActive)
            {
                response.HasError = true;
                response.Error = $"Cuenta no activada para '{request.UserName}', comuníquese con el administrador.";
                return response;
            }

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Credenciales incorrectas.";
                return response;
            }

            response.Id = user.Id;
            response.UserName = user.UserName;
            response.IsActive = user.IsActive;
            var roleList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            response.Roles = roleList.ToList();

            return response;
        }
        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        public async Task<List<UserDTO?>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var userDTOs = new List<UserDTO?>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);

                userDTOs.Add(new UserDTO
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.PasswordHash,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IdentificationCard = user.IdentificationCard,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    Role = roles.ToList(),
                    IsActive = user.IsActive
                });
            }

            return userDTOs;
        }

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Email}";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new Core.Application.DTOs.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your account visiting this URL {verificationUri}",
                Subject = "reset password"
            });


            return response;
        }

        public async Task<RegisterResponse> RegisterUserAsync(RegisterRequest request)
        {
            RegisterResponse response = new RegisterResponse()
            {
                HasError = false
            };

            var userWithUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithUserName != null)
            {
                response.HasError = true;
                response.Error = $"El nombre de usuario '{request.UserName}' ya está registrado.";
                return response;
            }

            var userWithEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithEmail != null)
            {
                response.HasError = true;
                response.Error = $"El email '{request.Email}' ya está registrado";
                return response;
            }

            if (!IsPasswordValid(request.Password))
            {
                response.HasError = true;
                response.Error = "La contraseña proporcionada no cumple con los requisitos.";
                return response;
            }

            var user = new ApplicationUser
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                IdentificationCard = request.IdentificationCard,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (result.Succeeded)
            {
                if (request.Role == Roles.Admin.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                }
                else if (request.Role == Roles.Client.ToString())
                {
                    await _userManager.AddToRoleAsync(user, Roles.Client.ToString());
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"Un error ocurrió mientras se registraba la cuenta.";
                return response;
            }

            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        // Desde el UserService o lo que sea, se hace la validación de que el mismo usuario no puede cambiar su estado
        public async Task ToogleUserActiveStatusAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                user.IsActive = !user.IsActive;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task<UpdateUserResponse> UpdateUserAsync(UpdateUserRequest request)
        {
            var user = await _userManager.FindByIdAsync(request.Id);

            if (user == null)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = $"No se encontraron usuarios con el ID: {request.Id}"
                };
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.IdentificationCard = request.IdentificationCard;
            user.Email = request.Email;

            if (request.Password != null)
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, request.Password);
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return new UpdateUserResponse
                {
                    HasError = true,
                    Error = "Error actualizando la información del usuario."
                };
            }

            return new UpdateUserResponse();
        }

        private bool IsPasswordValid(string password)
        {
            PasswordOptions opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            if (string.IsNullOrEmpty(password) || password.Length < opts.RequiredLength)
                return false;

            int uniqueChars = password.Distinct().Count();

            if (uniqueChars < opts.RequiredUniqueChars)
                return false;

            if (opts.RequireDigit && !password.Any(char.IsDigit))
                return false;

            if (opts.RequireLowercase && !password.Any(char.IsLower))
                return false;

            if (opts.RequireUppercase && !password.Any(char.IsUpper))
                return false;

            if (opts.RequireNonAlphanumeric && password.All(char.IsLetterOrDigit))
                return false;

            return true;
        }
        private async Task<string> SendVerificationEmailUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);

            return verificationUri;
        }
        private async Task<string> SendForgotPasswordUri(ApplicationUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);

            return verificationUri;
        }
    }
}
