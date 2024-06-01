using System.ComponentModel.DataAnnotations;

namespace ItlaBankApp.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "Debe colocar el nombre del usuario")]
        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Debe colocar el apellido del usuario")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Debe colocar un nombre de usuario")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coiciden")]
        [Required(ErrorMessage = "Debe colocar una contraseña")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Debe colocar un correo")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Debe colocar un número de teléfono")]
        [DataType(DataType.Text)]
        public string PhoneNumber { get; set; }

        [DataType(DataType.Text)]
        public string IdentificationCard { get; set; }

        [Required(ErrorMessage = "Debe específicar el rol del usuario")]
        public string Role { get; set; }

        [DataType(DataType.Currency)]
        public decimal? Amount { get; set; }

        public bool IsActive { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }

    }

}