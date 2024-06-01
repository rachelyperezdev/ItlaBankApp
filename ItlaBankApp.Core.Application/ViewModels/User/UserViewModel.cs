namespace ItlaBankApp.Core.Application.ViewModels.User
{
    public class UserViewModel
    {
        public string id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } 
        public string IdentificationCard { get; set; }
        public List<string> Role { get; set; }
        public bool IsActive { get; set; }

    }

}