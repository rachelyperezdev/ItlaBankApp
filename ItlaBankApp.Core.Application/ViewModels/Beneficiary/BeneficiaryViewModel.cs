using ItlaBankApp.Core.Application.DTOs.Account;
using ItlaBankApp.Core.Application.ViewModels.Product;
using ItlaBankApp.Core.Application.ViewModels.User;

namespace ItlaBankApp.Core.Application.ViewModels.Beneficiary
{
    public class BeneficiaryViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BeneficiaryId { get; set; }
        public UserViewModel Beneficiary { get; set; }
        public int AccountId { get; set; }
        public ProductViewModel Account { get; set; }
    }
}
