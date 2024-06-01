using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.ViewModels.Product;
using Microsoft.AspNetCore.Mvc;
using ItlaBankApp.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;

namespace ItlaBankApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public ProductController(IProductService productService, IUserService userService)
        {
            _productService = productService;
            _userService = userService;
        }
        public async Task<IActionResult> Index(string id)
        {
            var products = await _productService.GetAllUserAccounts(id);
            return View(products);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _productService.GetByIdSaveViewModel(id);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            var vm = await _productService.GetByIdSaveViewModel(id);

            if (vm.IsMain == true)
            {
                TempData["IsMainMessage"] = "No puedes Eliminar tu Cuenta Principal!!";
                return RedirectToRoute(new { controller = "User", action = "AdminUsers" });
            }
            if (vm.Debt != null && vm.Debt != 0)
            {
                TempData["DebeDinero"] = "No puedes eliminar esta cuenta porque el cliente tiene una deuda pendiente";
                return RedirectToRoute(new { controller = "User", action = "AdminUsers" });
            }
            if (vm.ProductType.Value == ProductType.Savings)
            {
                if (vm.Amount.HasValue && vm.Amount.Value != 0)
                {
                    var cuentaMain = await _productService.GetMainAccount(vm.UserId);
                    cuentaMain.Amount += vm.Amount;
                    await _productService.Update(cuentaMain, cuentaMain.AccountId);


                }
            }
            await _productService.Delete(id);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }

        public IActionResult Create(ProductType type, string id)
        {
            var vm = new SaveProductViewModel();
            vm.ProductType = type;
            vm.UserId = id;
            return View("Create", vm);
        }


        [HttpPost]
        public async Task<IActionResult> Create(SaveProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("SaveComment", vm);
            }

            var user = await _userService.GetUserById(vm.UserId);

            if(user.Username == "defaultClient" && vm.ProductType != ProductType.Savings)
            {
                var savingAccounts = await _productService.GetAllUserSavingsAccounts(vm.UserId);

                if (savingAccounts.Count == 0)
                {
                    SaveProductViewModel savingAccount = new()
                    {
                        UserId = vm.UserId,
                        IsMain = true,
                        Amount = 0,
                        ProductType = ProductType.Savings
                    };
                    await _productService.AddSavingAccount(savingAccount);
                }
            }

            if (vm.ProductType == ProductType.Savings)
            {
                if (user.Username == "defaultClient")
                {
                    var savingAccounts = await _productService.GetAllUserSavingsAccounts(vm.UserId);
                    if (savingAccounts.Count == 0)
                    {
                        vm.IsMain = true;
                    }
                }

                await _productService.AddSavingAccount(vm);
            }
            else if (vm.ProductType == ProductType.CreditCard)
            {
                await _productService.AddCreditCard(vm);
            }
            else if (vm.ProductType == ProductType.Loan)
            {
                await _productService.AddLoan(vm);
            }
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
    }
}