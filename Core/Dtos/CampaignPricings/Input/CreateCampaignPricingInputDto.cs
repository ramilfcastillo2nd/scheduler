using System;

namespace Core.Dtos.CampaignPricings.Input
{
    public class CreateCampaignPricingInputDto
    {
        public string Name { get; set; }
        public int? DuarationId { get; set; }
        public decimal? Pricing { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
}
