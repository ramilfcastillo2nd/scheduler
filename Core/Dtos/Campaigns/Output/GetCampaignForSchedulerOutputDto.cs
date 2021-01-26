using System;

namespace Core.Dtos.Campaigns.Output
{
    public class GetCampaignForSchedulerOutputDto
    {
        public string CampaignName { get; set; }
        public string Plan { get; set; }
        public string Client { get; set; }
        public string ClientEmail { get; set; }
        public int? AccountsExecutiveId { get; set; }
        public int? TrelloEditorId { get; set; }
        public DateTime? Started { get; set; }
        public ContentOutputDto Content { get; set; }
    }
}
