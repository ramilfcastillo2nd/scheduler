namespace Core.Dtos.Wages.Input
{
    public class UpdateWageInputDto
    {
        public int Id { get; set; }
        public string SdrType { get; set; }
        public string CampaignType { get; set; }
        public decimal BasePay { get; set; }
        public decimal IncentivePay { get; set; }
    }
}
