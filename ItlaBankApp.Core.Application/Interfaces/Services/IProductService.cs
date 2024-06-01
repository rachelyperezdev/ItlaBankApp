using ItlaBankApp.Core.Application.Interfaces.Services.Generic;
using ItlaBankApp.Core.Application.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Domain.Entities;

namespace ItlaBankApp.Core.Application.Interfaces.Services
{
    public interface IProductService : IGenericService<SaveProductViewModel, ProductViewModel, Product>
    {
        Task AddSavingAccount(SaveProductViewModel vm);
        Task AddCreditCard(SaveProductViewModel vm);
        Task AddLoan(SaveProductViewModel vm);
        Task<List<ProductViewModel>> GetAllCurrentUserCreditCards();
        Task<List<ProductViewModel>> GetAllCurrentUserLoans();
        Task<List<ProductViewModel>> GetAllCurrentUserSavingsAccounts();
        Task<ProductViewModel> GetByAccount(int accountNumber);
        Task<bool> ValidateAmount(int productId, decimal amount);
        Task<bool> ValidateNoDebt(int productId);
        Task<List<ProductViewModel>> GetAllCurrentUserAccounts();
        Task<List<ProductViewModel>> GetAllUserAccounts(string id);
        Task<List<ProductViewModel>> GetAllProductsByUserId(string userId);
        Task<int> GetProductsQty();
        Task<List<ProductViewModel>> GetAllUserSavingsAccounts(string userId);
        Task<SaveProductViewModel> GetMainAccount(string userId);
    }
}
