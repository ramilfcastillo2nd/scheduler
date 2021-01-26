using System;

namespace Core.Dtos.Account.Output
{
    public class UserAccountOutputDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int StatusId { get; set; }
    }
}
