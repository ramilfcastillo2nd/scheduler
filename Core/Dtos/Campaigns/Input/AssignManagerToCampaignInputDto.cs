using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Campaigns.Input
{
    public class AssignManagerToCampaignInputDto
    {
        public int CampaignId { get; set; }
        public int UserProfileId { get; set; }
    }
}
