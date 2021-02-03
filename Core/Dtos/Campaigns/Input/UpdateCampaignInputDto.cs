using System;

namespace Core.Dtos.Campaigns.Input
{
    public class UpdateCampaignInputDto
    {
        public int id { get; set; }
        public string CampaignName { get; set; }
        public string Plan { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; }
        public int? AccountsExecutiveId { get; set; }
        public int? TrelloEditorId { get; set; }
        public DateTime? Started { get; set; }
        public int? StatusId { get; set; } = 1;
        public int? CampaignTypeId { get; set; }
    }
}
