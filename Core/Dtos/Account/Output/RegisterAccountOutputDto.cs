using System;

namespace Core.Dtos.Account.Output
{
    public class RegisterAccountOutputDto
    {
        public Guid? UserId { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
