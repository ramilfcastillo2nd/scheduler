using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignAssignmentListByUserProfileIdSpecification : BaseSpecification<CampaignAssignment>
    {
        public GetCampaignAssignmentListByUserProfileIdSpecification(int userProfileId)
              : base(x =>
                 (x.UserProfileId == userProfileId)
            )
        {
            AddInclude(x => x.Campaign);
        }
    }
}
