using Core.Entities;

namespace Core.Specifications
{
    public class GetSchedulerCountSpecification: BaseSpecification<Scheduler>
    {
        public GetSchedulerCountSpecification(CommonSpecParams specParams, int campaignId, int userProfileId)
            : base(x =>
                 (x.UserProfileId == userProfileId) &&
                 (x.CampaignId == campaignId) &&
                 (
                     (x.FirstName.Contains(specParams.Search)) ||
                     (x.LastName.Contains(specParams.Search)) ||
                     (x.Company.Contains(specParams.Search)) ||
                     (string.IsNullOrEmpty(specParams.Search))
                 )
            )
        {
         
        }
    }
}
