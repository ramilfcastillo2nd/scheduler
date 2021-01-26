using Core.Dtos.Payrolls.Input;
using Core.Entities;

namespace Core.Specifications
{
    public class GetSchedulerByUserAndCampaignSpecification: BaseSpecification<Scheduler>
    {
        public GetSchedulerByUserAndCampaignSpecification(ProcessPayrollInputDto request, int campaignId)
            : base(x =>
                 (x.UserProfileId == request.EmployeeId) &&
                 (x.CampaignId == campaignId) &&
                 (
                    (x.Date.HasValue || (x.Date.Value >=request.StartDate && x.Date.Value <= request.EndDate)) ||
                    (x.DateMessage1.HasValue || (x.DateMessage1.Value >= request.StartDate && x.DateMessage1.Value <= request.EndDate)) ||
                    (x.DateMessage2.HasValue || (x.DateMessage2.Value >= request.StartDate && x.DateMessage2.Value <= request.EndDate)) ||
                    (x.DateMessage3.HasValue || (x.DateMessage3.Value >= request.StartDate && x.DateMessage3.Value <= request.EndDate)) ||
                    (x.DateMessage4.HasValue || (x.DateMessage4.Value >= request.StartDate && x.DateMessage4.Value <= request.EndDate)) ||
                    (x.DateMessage5.HasValue || (x.DateMessage5.Value >= request.StartDate && x.DateMessage5.Value <= request.EndDate))
                 )
            )
        {

        }
    }
}
