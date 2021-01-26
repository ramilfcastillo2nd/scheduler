using Core.Entities;

namespace Core.Specifications
{
    public class GetExistingCampaignAssignmentSpecification: BaseSpecification<CampaignAssignment>
    {
        public GetExistingCampaignAssignmentSpecification(int campaignId, int userProfileId)
            :base(x => 
                (x.CampaignId == campaignId) &&
                (x.UserProfileId == userProfileId)
            )
        {

        }
    }
}
