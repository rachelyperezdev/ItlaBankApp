using ItlaBankApp.Core.Application.ViewModels.Product;
using System.ComponentModel.DataAnnotations;

namespace ItlaBankApp.Core.Application.ViewModels.Beneficiary
{
    public class SaveBeneficiaryViewModel
    {   
        public int Id { get; set; }

        [Range(100000000, 999999999, ErrorMessage = "Debe insertar un número de cuenta de 9 dígitos.")]
        public int AccountId { get; set; }
        public string? UserId { get; set; }
        public string? BeneficiaryId { get; set; }

        public string? ErrorMessage { get; set; }
    }
}
