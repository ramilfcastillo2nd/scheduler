using System;

namespace Core.Dtos.Account.Output
{
    public class LoginAccountOutputDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string TimeZone { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public bool HasChangePassword { get; set; }
        public int Role { get; set; }
    }
}
