namespace ItlaBankApp.Core.Application.DTOs.Account
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentificationCard { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Role { get; set; }
        public bool IsActive { get; set; }
    }
}