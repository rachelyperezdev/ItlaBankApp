using ItlaBankApp.Core.Application.Interfaces.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Payment;
using ItlaBankApp.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ItlaBankApp.Core.Application.Interfaces.Services
{
    public interface IPaymentService : IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task<List<SavePaymentViewModel>> GetAllUserPaymentsAsync();
        Task MakeExpressPayment(int originId, decimal amount, int destinyId);
        Task MakeCreditCardPayment(int originId, decimal amount, int creditCardId);
        Task MakeLoanPayment(int originId, decimal amount, int loanId);
        Task MakeBeneficiaryPayment(int originId, decimal amount, int beneficiaryId);
        Task MakeTransfer(int originId, decimal amount, int accountId);
        Task MakeCashAdvance(int creditCardId, decimal amount, int accountId);
        Task<int> GetAllTimePayments();
        Task<int> GetDailyPayments();
        Task<int> GetDailyTransactions();
        Task<int> GetAllTimeTransactions();
    }
}
