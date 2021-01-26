using Core.Entities;
using Core.Enums;

namespace Core.Specifications
{
    public class GetActiveCampaignsSpecification: BaseSpecification<Campaign>
    {
        public GetActiveCampaignsSpecification()
            :base(x => 
                (x.StatusId == (int)CampaignStatus.Active)
            )
        {

        } 
    }
}
