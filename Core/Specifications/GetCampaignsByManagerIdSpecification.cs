using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignsByManagerIdSpecification: BaseSpecification<Campaign>
    {
        public GetCampaignsByManagerIdSpecification(int userProfileId)
            : base(x => 
                (x.UserProfileId == userProfileId) 
            )
        {

        }
    }
}
