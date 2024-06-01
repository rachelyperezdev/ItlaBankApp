namespace ItlaBankApp.Core.Application.DTOs.Account
{
    public class RegisterRequest
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentificationCard { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        //public decimal? InitialAmount { get; set; }
    }
}