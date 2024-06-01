using ItlaBankApp.Core.Application.Enums;
using System.ComponentModel.DataAnnotations;

namespace ItlaBankApp.Core.Application.ViewModels.Payment
{
    public class SavePaymentViewModel
    {
        [Required(ErrorMessage = "Debe ingresar un origen.")]
        [NotZero]
        public int OriginId { get; set; }

        [Required(ErrorMessage = "Debe ingresar una cuenta destino.")]
        [NotZero]
        public string DestinationId { get; set; }
        public string? FullNameDestino { get; set; }

        [Required(ErrorMessage = "Debe ingresar un monto.")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }
        public string? ErrorMessage { get; set; }
        public string? HolderFullName { get; set; }
        public PaymentType? PaymentType { get; set; }
    }

    public class NotZeroAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                string stringValue = value.ToString().Trim();
                if (stringValue == "0" && validationContext.DisplayName == "OriginId")
                {
                    return new ValidationResult("Debe ingresar una cuenta de origen.");
                }
                else if (stringValue == "0" && validationContext.DisplayName == "DestinationId")
                {
                    return new ValidationResult("Debe ingresar una cuenta de destino.");
                }
            }

            return ValidationResult.Success;
        }
    }
}