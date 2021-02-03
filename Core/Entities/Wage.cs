namespace Core.Entities
{
    public class Wage : BaseEntity
    {
        public string SdrType { get; set; }
        public string CampaignType { get; set; }
        public decimal BasePay { get; set; }
        public decimal? IncentivePay { get; set; }
    }
}
