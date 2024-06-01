using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Enums;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.ItlaBankApp.Middlewares;

namespace ItlaBankApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public UserController(IUserService userService, IProductService productService)
        {
            _userService = userService;
            _productService = productService;
        }
         
        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {
            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(vm);
            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { controller = "Home", action = "Index" });
            }
            else
            {
                vm.HasError = userVm.HasError;
                vm.Error = userVm.Error;
                return View(vm);
            }
        }


        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Register()
        {
            if (!User.IsInRole("Admin"))
            {
                // Redirigir al usuario a una página de error o cualquier otra acción adecuada.
                return RedirectToRoute(new { controller = "User", action="Index" });
            }

            return View(new SaveUserViewModel());
        }

        //[ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Register(SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var origin = Request.Headers["origin"];
         

            RegisterResponse response = await _userService.RegisterAsync(vm, origin);

            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }

            var user = await _userService.GetUserByUsernameSaveAsync(vm.Username);

            if (vm.Role == Roles.Client.ToString())
            {
                SaveProductViewModel savingAccount = new()
                {
                    ProductType = ProductType.Savings,
                    Amount = vm.Amount,
                    IsMain = true,
                    UserId = user.Id
                };

                await _productService.AddSavingAccount(savingAccount);
            }

            return RedirectToRoute(new { controller = "User", action = "AdminUsers" });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string username)
        {
            var user = await _userService.GetUserByUsernameAsync(username);
            var productos = await _productService.GetAllProductsByUserId(user.id);
            bool tieneDeuda = false;

            foreach (var producto in productos)
            {
                if (producto.Debt != null && producto.Debt != 0)
                {
                    tieneDeuda = true;
                    break;
                }
            }

            if (tieneDeuda)
            {
                TempData["IsMainMessage"] = "Este usuario tiene productos pendientes de pago. No se puede eliminar.";
                return RedirectToAction("AdminUsers", "User");
            }
            return View(await _userService.GetUserByUsernameAsync(username));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeletePost(string id)
        {

            await _userService.DeleteUserAsync(id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string UserName)
        {
            SaveUserViewModel vm = await _userService.GetUserByUsernameSaveAsync(UserName);
            return View("Edit", vm);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Edit(SaveUserViewModel vm)
        {
            SaveUserViewModel UserVm = await _userService.GetUserByUsernameSaveAsync(vm.Username);

            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(vm.Password) && string.IsNullOrEmpty(vm.ConfirmPassword))
                {
                    vm.Password = UserVm.Password;
                    vm.ConfirmPassword = UserVm.ConfirmPassword;
                    ModelState.Remove("Password");
                    ModelState.Remove("ConfirmPassword");
                }
                if (string.IsNullOrEmpty(vm.Role))
                {
                    ModelState.Remove("Role");
                }

            }
            if (!ModelState.IsValid)
            {
                return View("Edit", vm);
            }

            var mainSavingAccount = await _productService.GetMainAccount(UserVm.Id);

            if(mainSavingAccount == null)
            {
                SaveProductViewModel newMainSavingAccount = new()
                {
                    Amount = vm.Amount,
                    ProductType = ProductType.Savings,
                    IsMain = true,
                    UserId = UserVm.Id
                };

                await _productService.AddSavingAccount(newMainSavingAccount);
                mainSavingAccount = await _productService.GetMainAccount(newMainSavingAccount.UserId);
            }
            else
            {
                mainSavingAccount.Amount += vm.Amount;
            }
            
            vm.IsActive = UserVm.IsActive;
            await _userService.UpdateUserAsync(vm.Email, vm);
            await _productService.Update(mainSavingAccount, mainSavingAccount.AccountId);

            return RedirectToAction("AdminUsers", "User");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ToggleActiveStatus(string UserName)
        {
            SaveUserViewModel vm = await _userService.GetUserByUsernameSaveAsync(UserName);

            vm.IsActive = !vm.IsActive;

            await _userService.UpdateUserAsync(vm.Email, vm);

            return RedirectToAction("AdminUsers", "User");
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ForgotPassword()
        {
            return View(new ForgotPasswordViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var origin = Request.Headers["origin"];
            ForgotPasswordResponse response = await _userService.ForgotPasswordAsync(vm, origin);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult ResetPassword(string token)
        {
            return View(new ResetPasswordViewModel { Token = token });
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ResetPasswordResponse response = await _userService.ResetPasswordAsync(vm);
            if (response.HasError)
            {
                vm.HasError = response.HasError;
                vm.Error = response.Error;
                return View(vm);
            }
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [Authorize(Roles = "Admin,Client")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AdminUsers()
        {
            return View(await _userService.GetAll());
        }
    }
}
