using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Content: BaseEntity
    {
        public string InviteMessage { get; set; }
        public string Message1 { get; set; }
        public string Message2 { get; set; }
        public string Message3 { get; set; }
        public string Message4 { get; set; }
        public string Message5 { get; set; }
        public int CampaignId { get; set; }
        [ForeignKey("CampaignId")]
        public Campaign Campaign { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy{ get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
