using Core.Entities.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class UserProfile: BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string LinkedInUrl { get; set; }
        public string LinkedInName { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public decimal? Wage { get; set; }
        public int? StatusId { get;set; }
        public int? DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
        public DateTime? DateHired { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ImageUrl { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }
    }
}
