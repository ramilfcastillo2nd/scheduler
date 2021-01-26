using System;

namespace Core.Dtos.Account.Input
{
    public class LoginAccountInputDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string TimeZone { get; set; }
        public DateTime? LastLocalTimeLoggedIn { get; set; }
    }
}
