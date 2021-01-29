namespace Core.Dtos.CampaignPricings.Input
{
    public class UpdateCampaignPricingInputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DuarationId { get; set; }
        public decimal? Pricing { get; set; }
    }
}
