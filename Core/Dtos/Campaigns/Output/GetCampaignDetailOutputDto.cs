using System;

namespace Core.Dtos.Campaigns.Output
{
    public class GetCampaignDetailOutputDto
    {
        public string CampaignName { get; set; }
        public string Plan { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; }
        public int? AccountsExecutiveId { get; set; }
        public int? TrelloEditorId { get; set; }
        public DateTime? Started { get; set; }
        public int? StatusId { get; set; }
        public int? UserProfileId { get; set; }
        public int? CampaignTypeId { get; set; }
        public ManagerDetailsOutputDto Manager { get; set; }
    }
}
