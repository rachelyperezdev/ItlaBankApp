using ItlaBankApp.Core.Domain.Common;

namespace ItlaBankApp.Core.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int OriginId { get; set; } 
        public Product Origin { get; set; }
        public decimal Amount { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BeneficiaryId { get; set; } // Cuando el destino es un beneficiario
        public Beneficiary? Beneficiary { get; set; } 
        public int? DestinationId { get; set; } // Cuando el destino es un producto de la misma cuenta (pago tarjeta, prestamo, avance de efectivo, transferencia)
        public Product? Destination { get; set; }
        public int? AccountId { get; set; } // Cuando el destino es un número de cuenta (pago expreso)
    }

    public enum PaymentType
    {
        Express, // Pago Expreso
        CreditCard, // Pago a Tarjeta de Crédito
        Loan, // Pago a Préstamo
        Beneficiary, // Pago a Beneficiario
        CashAdvance, // Avance de Efectivo
        Transfer // Transferencia a mis cuentas
    }
}