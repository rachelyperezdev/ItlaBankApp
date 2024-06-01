using ItlaBankApp.Core.Application.Enums;

namespace ItlaBankApp.Core.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int AccountId { get; set; }
        public ProductType? ProductType { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsMain { get; set; } 
        public decimal? Debt { get; set; }
        public string? UserId { get; set; }
        public string? HolderFullName { get; set; }
    }
}