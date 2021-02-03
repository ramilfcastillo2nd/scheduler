namespace Core.Dtos.Wages.Output
{
    public class GetWageOutputDto
    {
        public int id { get; set; }
        public string SdrType { get; set; }
        public string CampaignType { get; set; }
        public decimal BasePay { get; set; }
        public decimal? IncentivePay { get; set; }
    }
}
