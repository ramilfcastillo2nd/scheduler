using Core.Entities.Identity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class Campaign: BaseEntity
    {
        public string CampaignName { get; set; }
        public int CampaignType { get; set; }
        public string Plan { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; } 
        public int? AccountsExecutiveId { get; set; }
        [ForeignKey("AccountsExecutiveId")]
        public UserProfile AccountsExecutive { get; set; }
        public int? TrelloEditorId { get; set; }
        [ForeignKey("TrelloEditorId")]
        public UserProfile TrelloEditor { get; set; }
        public DateTime? Started { get; set; }
        public int? StatusId { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? UserProfileId { get; set; }
        [ForeignKey("UserProfileId")]
        public UserProfile UserProfile { get; set; }
        public ICollection<CampaignAssignment> CampaignAssignments { get; set; }
    }
}
