using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Dtos.Account.Input
{
    public class UserUpdateInputDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string MobilePhone { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
