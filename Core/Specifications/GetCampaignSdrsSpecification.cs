using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignSdrsSpecification: BaseSpecification<CampaignAssignment>
    {
        public GetCampaignSdrsSpecification(int campaignId)
                    : base(x =>
                         (x.CampaignId == campaignId)
                    )
        {
            AddInclude(x => x.UserProfile);
        }
    }
}
