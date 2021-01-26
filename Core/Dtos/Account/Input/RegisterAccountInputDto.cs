using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Account.Input
{
    public class RegisterAccountInputDto
    {
        public string FirstName  { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public int RoleId { get; set; }
    }
}
