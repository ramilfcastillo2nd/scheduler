using Core.Entities;
using Core.Enums;

namespace Core.Specifications
{
    public class GetCampaignAssignmentByUserProfileIdSpecification: BaseSpecification<CampaignAssignment>
    {
        public GetCampaignAssignmentByUserProfileIdSpecification(int userProfileId)
            : base(x =>
                 (x.UserProfileId == userProfileId) &&
                 (x.StatusId == (int)AssignedSdrStatus.Active)
            )
        {
            AddInclude(s => s.Campaign);
        }
    }
}
