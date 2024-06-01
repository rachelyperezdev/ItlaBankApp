using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using ItlaBankApp.Core.Application.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace ItlaBankApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPaymentService _paymentService;
        private readonly IProductService _productService;
        private readonly AuthenticationResponse _currentUser;
        public HomeController(
            IUserService userService, 
            IHttpContextAccessor httpContextAccessor,
            IPaymentService paymentService,
            IProductService productService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _paymentService = paymentService;
            _productService = productService;
            _currentUser = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userService.GetUserById(_currentUser.Id);

            ViewBag.AllTimeTransactions = await _paymentService.GetAllTimeTransactions();
            ViewBag.DailyTransactions = await _paymentService.GetDailyTransactions();

            ViewBag.AllTimePayments = await _paymentService.GetAllTimePayments();
            ViewBag.DailyPayments = await _paymentService.GetDailyPayments();

            ViewBag.Products = await _productService.GetProductsQty();

            ViewBag.ActiveClients = await _userService.GetActiveClients();
            ViewBag.InactiveClients = await _userService.GetInactiveClients();

            ViewBag.CurrentUserProducts = await _productService.GetAllCurrentUserAccounts();

            return View(user);
        }


    }
}
