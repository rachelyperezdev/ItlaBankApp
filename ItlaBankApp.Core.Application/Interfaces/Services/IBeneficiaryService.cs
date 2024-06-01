using ItlaBankApp.Core.Application.Interfaces.Services.Generic;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Domain.Entities;

namespace ItlaBankApp.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService : IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        Task PopulateBeneficiaryData(BeneficiaryViewModel viewModel);
        Task<ProductViewModel> GetProductByAccount(int accountNumber);
        Task<List<BeneficiaryViewModel>> GetAllCurrentUserBeneficiaries();
    }
}
