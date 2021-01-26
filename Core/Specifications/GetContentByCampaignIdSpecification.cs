using Core.Entities;

namespace Core.Specifications
{
    public class GetContentByCampaignIdSpecification: BaseSpecification<Content>
    {
        public GetContentByCampaignIdSpecification(int campaignId)
            :base(x =>
                (x.CampaignId == campaignId)
            )
        {

        }
    }
}
