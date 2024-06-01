using AutoMapper;
using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.Interfaces.Repositories;
using ItlaBankApp.Core.Application.Interfaces.Services;
using ItlaBankApp.Core.Application.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Payment;
using ItlaBankApp.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace ItlaBankApp.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel, PaymentViewModel, Payment>, IPaymentService
    {
        private readonly IPaymentRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private IHttpContextAccessor _httpContextAccessor;
        private IUserService _userService;
        private AuthenticationResponse _userViewModel;

        public PaymentService(IPaymentRepository repository, IMapper mapper, IUserService userService, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor) : base(repository, mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _userService = userService;
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> GetAllTimePayments()
        {
            return await _repository.GetAllTimePayments();
        }

        public async Task<List<SavePaymentViewModel>> GetAllUserPaymentsAsync()
        {
            var payments = await _repository.GetAllAsync();
            var currentUserPayments = payments.Where(b => b.Origin.UserId == _userViewModel.Id).ToList();

            return _mapper.Map<List<SavePaymentViewModel>>(currentUserPayments);
        }

        public async Task<int> GetDailyPayments()
        {
            return await _repository.GetDailyPayments();
        }

        public async Task<int> GetAllTimeTransactions()
        {
            return await _repository.GetAllTimeTransactions();
        }

        public async Task<int> GetDailyTransactions()
        {
            return await _repository.GetDailyTransactions();
        }

        public async Task MakeBeneficiaryPayment(int originId, decimal amount, int beneficiaryId)
        {
            var originAccount = await _productRepository.GetByAccountAsync(originId);
            var beneficiaryAccount = await _productRepository.GetByAccountAsync(beneficiaryId);

            originAccount.Amount -= amount;
            beneficiaryAccount.Amount += amount;

            // record of every payment
            Payment payment = new Payment
            {
                OriginId = originId,
                DestinationId = beneficiaryId,
                Amount = amount,
                PaymentType = PaymentType.Beneficiary
            };

            await _repository.AddAsync(payment);
        }

        public async Task MakeCashAdvance(int creditCardId, decimal amount, int accountId)
        {
            var creditCard = await _productRepository.GetByAccountAsync(creditCardId);
            var account = await _productRepository.GetByAccountAsync(accountId);

            account.Amount += amount;
            creditCard.Debt += amount + (amount * 0.0625m);
            creditCard.Amount -= amount;

            Payment payment = new Payment
            {
                OriginId = creditCardId,
                DestinationId = accountId,
                Amount = amount,
                PaymentType = PaymentType.CashAdvance
            };

            await _productRepository.UpdateAsync(account, accountId);
            await _productRepository.UpdateAsync(creditCard, creditCardId);
            await _repository.AddAsync(payment);
        }

        public async Task MakeCreditCardPayment(int originId, decimal amount, int creditCardId)
        {
            var origin = await _productRepository.GetByAccountAsync(originId);
            var creditCard = await _productRepository.GetByAccountAsync(creditCardId);

            decimal debt = creditCard.Debt ?? 0;
            decimal amountToDebit = amount > debt ? debt : amount;

            origin.Amount -= amountToDebit;
            creditCard.Debt -= amountToDebit;

            // record of every payment
            Payment payment = new Payment
            {
                OriginId = originId,
                DestinationId = creditCardId,
                Amount = amountToDebit,
                PaymentType = PaymentType.CreditCard
            };

            await _productRepository.UpdateAsync(origin, originId);
            await _productRepository.UpdateAsync(creditCard, creditCardId);
            await _repository.AddAsync(payment);
        }

        public async Task MakeExpressPayment(int originId, decimal amount, int destinationId)
        {
            var originAccount = await _productRepository.GetByAccountAsync(originId);
            var destinationAccount = await _productRepository.GetByAccountAsync(destinationId);

            originAccount.Amount -= amount;
            destinationAccount.Amount += amount;

            // record of every payment
            Payment payment = new Payment
            {
                OriginId = originId,
                DestinationId = destinationId,
                Amount = amount,
                PaymentType = PaymentType.Express
            };

            await _repository.AddAsync(payment);
        }

        public async Task MakeLoanPayment(int originId, decimal amount, int loanId)
        {
            var origin = await _productRepository.GetByAccountAsync(originId);
            var loan = await _productRepository.GetByAccountAsync(loanId);

            decimal debt = loan.Debt ?? 0;
            decimal amountToDebit = amount > debt ? debt : amount;

            origin.Amount -= amountToDebit;
            loan.Debt -= amountToDebit;

            // record of every payment
            Payment payment = new Payment
            {
                OriginId = originId,
                DestinationId = loanId,
                Amount = amountToDebit,
                PaymentType = PaymentType.Loan
            };

            await _productRepository.UpdateAsync(origin, originId);
            await _productRepository.UpdateAsync(loan, loanId);
            await _repository.AddAsync(payment);
        }

        public async Task MakeTransfer(int originId, decimal amount, int accountId)
        {
            var originAccount = await _productRepository.GetByAccountAsync(originId);
            var destinationAccount = await _productRepository.GetByAccountAsync(accountId);

            originAccount.Amount -= amount;
            destinationAccount.Amount += amount;

            Payment payment = new Payment
            {
                OriginId = originId,
                DestinationId = accountId,
                Amount = amount,
                PaymentType = PaymentType.Transfer
            };

            await _productRepository.UpdateAsync(destinationAccount, accountId);
            await _productRepository.UpdateAsync(originAccount, originId);
            await _repository.AddAsync(payment);
        }

    }
}