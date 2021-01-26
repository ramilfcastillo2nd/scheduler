using Core.Entities;
using System;

namespace Core.Specifications
{
    public class GetPayrollForPeriodByUserSpecification: BaseSpecification<Payroll>
    {
        public GetPayrollForPeriodByUserSpecification(int userProfileId, int campaignId, DateTime startDate, DateTime endDate)
            :base(x =>
                (x.UserProfileId == userProfileId) &&
                (x.CampaignId == campaignId) &&
                (x.StartDate.Value.ToShortDateString() == startDate.ToShortDateString()) &&
                (x.EndDate.Value.ToShortDateString() == endDate.ToShortDateString())
            )
        {

        }
    }
}
