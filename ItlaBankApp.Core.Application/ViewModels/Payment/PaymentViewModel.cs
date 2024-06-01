using ItlaBankApp.Core.Application.Enums;
using ItlaBankApp.Core.Application.ViewModels.Beneficiary;
using ItlaBankApp.Core.Application.ViewModels.Product;

namespace ItlaBankApp.Core.Application.ViewModels.Payment
{
    public class PaymentViewModel
    {
        public int OriginId { get; set; }
        public ProductViewModel Origin { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BeneficiaryId { get; set; } 
        public BeneficiaryViewModel? Beneficiary { get; set; }
        public int? DestinationId { get; set; } 
        public ProductViewModel? Destination { get; set; }
        public int? AccountId { get; set; } 
    }
}