using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Account.Input
{
    public class RefreshTokenInputDto
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public string timeZone { get; set; } = "";
        public DateTime? lastLocalTimeLoggedIn { get; set; } = null;
    }
}
