namespace Core.Dtos.CampaignPricings.Output
{
    public class GetCampaignPricingOutputDto
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? DuarationId { get; set; }
        public decimal? Pricing { get; set; }
    }
}
