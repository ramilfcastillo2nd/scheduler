using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Attendance : BaseEntity
    {
        public int UserProfileId { get; set; }
        [ForeignKey("UserProfileId")]
        public UserProfile UserProfile { get; set; }
        public int CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int Points { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
