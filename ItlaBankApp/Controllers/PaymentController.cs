using ItlaBankApp.Core.Application.Enums;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.ViewModels.Payment;
using ItlaBankApp.Core.Application.ViewModels.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItlaBankApp.Controllers
{
    [Authorize(Roles = "Client")]
    public class PaymentController : Controller
    {
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        private readonly IBeneficiaryService _beneficiaryService;

        public PaymentController(IPaymentService paymentService, IUserService userService, IProductService productService, IBeneficiaryService beneficiaryService)
        {
            _userService = userService;
            _paymentService = paymentService;
            _productService = productService;
            _beneficiaryService = beneficiaryService;
        }

        /*public IActionResult Index()
        {
            return View();
        }*/

        #region Express
        public async Task<IActionResult> ExpressPayment()
        {
            ViewBag.OriginAccount = await _productService.GetAllCurrentUserSavingsAccounts();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ExpressPayment(SavePaymentViewModel vm)
        {
            ViewBag.OriginAccount = await _productService.GetAllCurrentUserSavingsAccounts();

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ProductViewModel destinationAccount = new();

            if (int.TryParse(vm.DestinationId, out int destinationId))
            {
                destinationAccount = await _productService.GetByAccount(destinationId);
            }
            else
            {
                TempData["ErrorMessage"] = $"No existe ninguna cuenta con este número de cuenta '{vm.DestinationId}'";
                return View(vm);
            }

            if (destinationAccount == null)
            {
                TempData["ErrorMessage"] = $"No existe ninguna cuenta con este número de cuenta '{vm.DestinationId}'";
                return View(vm);
            }

            if (destinationAccount.ProductType != ProductType.Savings)
            {
                TempData["ErrorMessage"] = $"Debe ingresar un número de cuenta de ahorro.";
                return View(vm);
            }

            if (vm.OriginId == destinationId)
            {
                TempData["ErrorMessage"] = "No puede transferir a la misma cuenta";
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                TempData["ErrorMessage"] = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                TempData["ErrorMessage"] = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            var originAccount = await _productService.GetByAccount(vm.OriginId);
            var originUser = await _userService.GetUserById(originAccount.UserId);
              
            vm.PaymentType = PaymentType.Express;
            vm.HolderFullName = originUser.FirstName + " " + originUser.LastName;
            return View("ConfirmPayment", vm);
        }
        #endregion

        #region Credit Card
        public async Task<IActionResult> CreditCardPayment()
        {
            ViewBag.SavingAccounts = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.CreditCards = await _productService.GetAllCurrentUserCreditCards();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreditCardPayment(SavePaymentViewModel vm)
        {
            ViewBag.SavingAccounts = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.CreditCards = await _productService.GetAllCurrentUserCreditCards();
            int destinationId = int.Parse(vm.DestinationId);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (await _productService.ValidateNoDebt(destinationId))
            {
                TempData["ErrorMessage"] = "Ya ha saldado su deuda, no debe pagar esta tarjeta de crédito.";
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                TempData["ErrorMessage"] = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                TempData["ErrorMessage"] = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            await _paymentService.MakeCreditCardPayment(vm.OriginId, vm.Amount, destinationId);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region Loan
        public async Task<IActionResult> LoanPayment()
        {
            ViewBag.SavingAccounts = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.Loans = await _productService.GetAllCurrentUserLoans();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> LoanPayment(SavePaymentViewModel vm)
        {
            ViewBag.SavingAccounts = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.Loans = await _productService.GetAllCurrentUserLoans();
            int destinationId = int.Parse(vm.DestinationId);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (await _productService.ValidateNoDebt(destinationId))
            {
                TempData["ErrorMessage"] = "Ya ha saldado su deuda, no debe pagar este préstamo.";
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                TempData["ErrorMessage"] = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                TempData["ErrorMessage"] = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            await _paymentService.MakeLoanPayment(vm.OriginId, vm.Amount, destinationId);

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region Beneficiary
        public async Task<IActionResult> BeneficiaryPayment()
        {
            ViewBag.Beneficiaries = await _beneficiaryService.GetAllCurrentUserBeneficiaries();
            ViewBag.Origins = await _productService.GetAllCurrentUserSavingsAccounts();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> BeneficiaryPayment(SavePaymentViewModel vm)
        {
            ViewBag.Beneficiaries = await _beneficiaryService.GetAllCurrentUserBeneficiaries();
            ViewBag.Origins = await _productService.GetAllCurrentUserSavingsAccounts();
            int destinationId = int.Parse(vm.DestinationId);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                TempData["ErrorMessage"] = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                TempData["ErrorMessage"] = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            var originAccount = await _productService.GetByAccount(vm.OriginId);
            var destinationAccount = await _productService.GetByAccount(destinationId);

            var userDestino = await _userService.GetUserById(destinationAccount.UserId);

            vm.PaymentType = PaymentType.Beneficiary;
            vm.HolderFullName = userDestino.FirstName + " " + userDestino.LastName;
            return View("ConfirmPayment", vm);
        }
        #endregion

        #region CashAdvance
        public async Task<IActionResult> CashAdvance()
        {
            ViewBag.Origin = await _productService.GetAllCurrentUserCreditCards();
            ViewBag.Destination = await _productService.GetAllCurrentUserSavingsAccounts();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CashAdvance(SavePaymentViewModel vm)
        {
            ViewBag.Origin = await _productService.GetAllCurrentUserCreditCards();
            ViewBag.Destination = await _productService.GetAllCurrentUserSavingsAccounts();


            int destinationId = int.Parse(vm.DestinationId);

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                vm.ErrorMessage = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                vm.ErrorMessage = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            vm.PaymentType = PaymentType.CashAdvance;

            await _paymentService.MakeCashAdvance(vm.OriginId, vm.Amount, destinationId);

            TempData["PaymentSucceed"] = "Avance de efectivo realizado exitosamente!";
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region Transfer
        public async Task<IActionResult> Transfer()
        {
            ViewBag.Origin = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.Destination = await _productService.GetAllCurrentUserSavingsAccounts();
            return View(new SavePaymentViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(SavePaymentViewModel vm)
        {
            ViewBag.Origin = await _productService.GetAllCurrentUserSavingsAccounts();
            ViewBag.Destination = await _productService.GetAllCurrentUserSavingsAccounts();

            int destinationId = int.Parse(vm.DestinationId);
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            if (vm.OriginId == destinationId)
            {
                vm.ErrorMessage = "No puede transferir a la misma cuenta";
                return View(vm);
            }

            if (!ValidateNoZeroAmount(vm.Amount))
            {
                vm.ErrorMessage = "El monto a pagar no puede ser $0.00";
                return View(vm);
            }

            if (!await _productService.ValidateAmount(vm.OriginId, vm.Amount))
            {
                vm.ErrorMessage = "Lo sentimos, no posee fondos suficientes en su cuenta para completar esta transacción.";
                return View(vm);
            }

            vm.PaymentType = PaymentType.Transfer;
            await _paymentService.MakeTransfer(vm.OriginId, vm.Amount, destinationId);

            TempData["PaymentSucceed"] = "Transferencia realizada exitosamente!";
            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region "Confirm Payment"

        [HttpPost]
        public async Task<IActionResult> ConfirmPaymentPost(SavePaymentViewModel vm)
        {
            var destinationId = int.Parse(vm.DestinationId);

            if (vm.PaymentType == PaymentType.Express)
            {
                await _paymentService.MakeExpressPayment(vm.OriginId, vm.Amount, destinationId);
            }
            else if (vm.PaymentType == PaymentType.Beneficiary)
            {
                await _paymentService.MakeBeneficiaryPayment(vm.OriginId, vm.Amount, destinationId);
            }

            return RedirectToRoute(new { controller = "Home", action = "Index" });
        }
        #endregion

        #region "Generic Methods"
        private bool ValidateNoZeroAmount(decimal amount)
        {
            if (amount == 0)
            {
                return false;
            }
            return true;
        }
        #endregion

    }
}

