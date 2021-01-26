using System;

namespace Core.Dtos.Account.Output
{
    public class CurrentUserOutputDto
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Image { get; set; }
        public string Role { get; set; }
    }
}
