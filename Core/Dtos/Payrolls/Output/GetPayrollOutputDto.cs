using System;

namespace Core.Dtos.Payrolls.Output
{
    public class GetPayrollOutputDto
    {
        public int UserProfileId { get; set; }
        public int CampaignId { get; set; }
        public bool? IsCancelled { get; set; }
        public int? DaysActive { get; set; }
        public int? IncentiveType { get; set; }
        public decimal? IncentiveAmount { get; set; }
        public int? IncentiveCount { get; set; }
        public decimal? Wage { get; set; }
        public decimal? BasePayAdjustment { get; set; }
        public decimal? ApptSalesIncentive { get; set; }
        public decimal? ReferralIncentive { get; set; }
        public decimal? OtherIncentive { get; set; }
        public decimal? RepliesInceentive { get; set; }
        public decimal? Total { get; set; }
        public decimal? SubTotal { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
