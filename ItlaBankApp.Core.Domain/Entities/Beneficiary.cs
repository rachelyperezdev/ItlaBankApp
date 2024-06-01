using ItlaBankApp.Core.Domain.Common;

namespace ItlaBankApp.Core.Domain.Entities
{
    public class Beneficiary : BaseEntity
    {
        // Nombre y apellido del beneficiario en el View Model
        public string UserId { get; set; } // User.Id == currentUser.Id   USUARIO LOGUEADO
        public string BeneficiaryId { get; set; } // Beneficiary.BeneficiaryId == Account.UserId  USUARIO BENEFICIARIO
        public int AccountId { get; set; } // En lugar de buscar el beneficiario por el Id de Producto, se busca por el número de cuenta
        public Product Account { get; set; }
    }
}
