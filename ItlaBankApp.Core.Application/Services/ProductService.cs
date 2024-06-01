using AutoMapper;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Helpers;
using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;
using ItlaBankApp.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ItlaBankApp.Core.Application.Services
{
    public class ProductService : GenericService<SaveProductViewModel, ProductViewModel, Product>, IProductService

    {
        private readonly IProductRepository _repository;
        private readonly IBeneficiaryRepository _beneficiaryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;
        private readonly AuthenticationResponse _userViewModel;

        public ProductService(IProductRepository productRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(productRepository, mapper)
        {
            _repository = productRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }

        private async Task<int> GenerateAccountNumber()
        {
            var random = new Random();
            int accountNumber;
            int existingAccount;
            bool accountFound;

            do
            {
                int randomNumber = random.Next(100000000, 999999999);
                accountNumber = randomNumber;

                var account = await _repository.GetByAccountAsync(accountNumber);

                if (account != null)
                {
                    existingAccount = account.AccountId;
                    accountFound = true;
                }
                else
                {
                    accountFound = false;
                    existingAccount = -1;
                }
            }
            while (existingAccount == accountNumber && !accountFound);

            return accountNumber;
        }
        public async Task<List<ProductViewModel>> GetAllProductsByUserId(string userId)
        {
            var allProducts = await _repository.GetAllAsync();
            var userProducts = allProducts.Where(p => p.UserId == userId).ToList();
            return _mapper.Map<List<ProductViewModel>>(userProducts);
        }

        public async Task<int> GetProductsQty()
        {
            var products = await _repository.GetAllAsync();
            return products.Count;
        }

        //Obtener todo por lista
        public async Task<List<ProductViewModel>> GetAllCurrentUserAccounts()
        {
            var allAccounts = await _repository.GetAllAsync();
            var currentUserAccounts = allAccounts
                                            .Where(p => p.UserId == _userViewModel.Id)
                                            .ToList();

            return _mapper.Map<List<ProductViewModel>>(currentUserAccounts);
        }

        public async Task<List<ProductViewModel>> GetAllUserAccounts(string id)
        {
            var allAccounts = await _repository.GetAllAsync();
            var currentUserAccounts = allAccounts
                                            .Where(p => p.UserId == id)
                                            .ToList();

            return _mapper.Map<List<ProductViewModel>>(currentUserAccounts);
        }

        public async Task<ProductViewModel> GetByAccount(int accountNumber)
        {
            var account = await _repository.GetByAccountAsync(accountNumber);
            ProductViewModel accountVm = _mapper.Map<ProductViewModel>(account);

            return accountVm;
        }

        private async Task<Product> GetUserMainAccount(string userId)
        {
            var accounts = await _repository.GetAllAsync();
            var mainAccount = accounts
                                .Where(p => p.UserId == userId &&
                                                                      p.IsMain == true).FirstOrDefault();

            return mainAccount;
        }

        public async Task<SaveProductViewModel> GetMainAccount(string userId)
        {
            var accounts = await _repository.GetAllAsync();
            var mainAccount = accounts
                                .Where(p => p.UserId == userId &&
                                       p.IsMain == true).FirstOrDefault();

            return _mapper.Map<SaveProductViewModel>(mainAccount);
        }

        public async Task<List<ProductViewModel>> GetAllCurrentUserCreditCards()
        {
            var creditCards = await _repository.GetAllAsync();
            var currentUserCreditCards = creditCards
                                                .Where(p => p.UserId == _userViewModel.Id &&
                                                       p.ProductType == ProductType.CreditCard).ToList();

            return _mapper.Map<List<ProductViewModel>>(currentUserCreditCards);
        }

        public async Task<List<ProductViewModel>> GetAllCurrentUserLoans()
        {
            var loans = await _repository.GetAllAsync();
            var currentUserLoans = loans
                                    .Where(loan => loan.UserId == _userViewModel.Id &&
                                           loan.ProductType == ProductType.Loan).ToList();

            return _mapper.Map<List<ProductViewModel>>(currentUserLoans);
        }

        public async Task<List<ProductViewModel>> GetAllCurrentUserSavingsAccounts()
        {
            var savingsAccounts = await _repository.GetAllAsync();
            var currentUserSavingsAccounts = savingsAccounts
                                                .Where(p => p.UserId == _userViewModel.Id &&
                                                       p.ProductType == ProductType.Savings).ToList();

            return _mapper.Map<List<ProductViewModel>>(currentUserSavingsAccounts);
        }

        public async Task<List<ProductViewModel>> GetAllUserSavingsAccounts(string userId)
        {
            var savingsAccounts = await _repository.GetAllAsync();
            var userSavingsAccounts = savingsAccounts
                                                .Where(p => p.UserId == userId &&
                                                       p.ProductType == ProductType.Savings).ToList();

            return _mapper.Map<List<ProductViewModel>>(userSavingsAccounts);
        }

        public async Task<bool> ValidateAmount(int productId, decimal amount)
        {
            var account = await _repository.GetByAccountAsync(productId);
            if (account.Amount >= amount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> ValidateNoDebt(int productId)
        {
            var account = await _repository.GetByAccountAsync(productId);
            if (account.Debt == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task AddSavingAccount(SaveProductViewModel vm)
        {
            decimal amount = vm.Amount ?? 0;

            Product savingAccount = new()
            {
                ProductType = (ProductType)vm.ProductType,
                Amount = amount,
                IsMain = vm.IsMain ?? false,
                AccountId = await GenerateAccountNumber(),
                UserId = vm.UserId
            };


            await _repository.AddAsync(savingAccount);
        }

        public async Task AddCreditCard(SaveProductViewModel vm)
        {
            decimal amount = vm.Amount ?? 0;

            Product creditCard = new Product()
            {
                ProductType = ProductType.CreditCard,
                Amount = amount,
                Debt = 0,
                IsMain = false,
                AccountId = await GenerateAccountNumber(),
                UserId = vm.UserId
            };

            await _repository.AddAsync(creditCard);
        }

        public async Task AddLoan(SaveProductViewModel vm)
        {
            decimal amount = vm.Amount ?? 0;

            Product loan = new Product()
            {
                ProductType = ProductType.Loan,
                Amount = amount,
                IsMain = false,
                Debt = amount,
                AccountId = await GenerateAccountNumber(),
                UserId = vm.UserId
            };

            var mainAccount = await GetUserMainAccount(vm.UserId);
            mainAccount.Amount += amount;
            await _repository.UpdateAsync(_mapper.Map<Product>(mainAccount), mainAccount.AccountId);

            await _repository.AddAsync(loan);
        }
    }
}
