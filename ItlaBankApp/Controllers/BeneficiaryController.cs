using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using ItlaBankApp.Core.Application.Enums;

namespace ItlaBankApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class BeneficiaryController : Controller
    {
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _user;

        public BeneficiaryController(IBeneficiaryService beneficiaryService, IUserService userService, 
                                    IHttpContextAccessor httpContextAccessor)
        {
            _beneficiaryService = beneficiaryService;
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _user = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index()
        {
            var beneficiaries = await _beneficiaryService.GetAllCurrentUserBeneficiaries();

            foreach (var beneficiary in beneficiaries)
            {
                await _beneficiaryService.PopulateBeneficiaryData(beneficiary);
            }

            return View(beneficiaries);
        }

        public IActionResult Create()
        {
            return View(new SaveBeneficiaryViewModel());
        }

        //[HttpPost]
        /*public async Task<IActionResult> Create(SaveBeneficiaryViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            vm.UserId = _user.Id;
            ProductViewModel account = await _beneficiaryService.GetProductByAccount(vm.AccountId);
            if(account == null)
            {
                vm.ErrorMessage = $"Cuenta inexistente '{vm.AccountId}'";
                return View(vm);
            }

            vm.BeneficiaryId = account.UserId;

            await _beneficiaryService.Add(vm);
            TempData["BeneficiarySucceed"] = "Beneficiario agregado exitosamente.";
            return RedirectToAction("Index");
        }*/

        [HttpPost]
        public async Task<IActionResult> Create(int accountId)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "No se pudo agregar el beneficiario.";
                return RedirectToAction("Index");
            }

            ProductViewModel account = await _beneficiaryService.GetProductByAccount(accountId);

            SaveBeneficiaryViewModel vm = new(); ;

            if (account == null)
            {
                TempData["ErrorMessage"] = "Cuenta inexistente.";
                return RedirectToAction("Index");
            }

            if(account.ProductType != ProductType.Savings)
            {
                TempData["ErrorMessage"] = "Los beneficiarios solo son cuentas de ahorro.";
                return RedirectToAction("Index");
            }

            vm.AccountId = accountId;
            vm.UserId = _user.Id;
            vm.BeneficiaryId = account.UserId;

            await _beneficiaryService.Add(vm);
            TempData["BeneficiarySucceed"] = "Beneficiario agregado exitosamente.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int id)
        {
            await _beneficiaryService.Delete(id);
            TempData["BeneficiarySucceed"] = "Beneficiario eliminado exitosamente.";
            return RedirectToRoute(new { controller = "Beneficiary", action = "Index" });
        }
    }
}
