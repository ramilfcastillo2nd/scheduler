using System;

namespace Core.Dtos.Campaigns.Output
{
    public class GetCampaignOutputDto
    {
        public int id { get; set; }
        public int SdrCount { get; set; }
        public string CampaignName { get; set; }
        public string Plan { get; set; }
        public string Client { get; set; }
        public DateTime? Started { get; set; }
        public int? StatusId { get; set; }
        public int? ManagerId { get; set; }
        public int? CampaignTypeId { get; set; }
    }
}
