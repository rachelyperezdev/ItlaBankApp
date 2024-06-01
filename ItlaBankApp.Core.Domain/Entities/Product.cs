using ItlaBankApp.Core.Domain.Common;

namespace ItlaBankApp.Core.Domain.Entities
{
    public class Product : AuditableBaseEntity
    {
        public int AccountId { get; set; }
        public ProductType ProductType { get; set; }
        public decimal Amount { get; set; }
        public bool IsMain { get; set; } = false; // Si se crea un nuevo usuario, se le asigna una cuenta principal
        public decimal? Debt { get; set; } // Deuda de la tarjeta de crédito o préstamo
        public string UserId { get; set; }  // Se le asigna el Id del usuario que lo creó o después de que se crea el usuario

    }

    public enum ProductType
    {
        Savings, // Cuenta de ahorros
        CreditCard, // Tarjeta de crédito
        Loan // Préstamo
    }
}
