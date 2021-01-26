using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignByIdWithDetailsSpecification: BaseSpecification<Campaign>
    {
        public GetCampaignByIdWithDetailsSpecification(int id)
            :base(x =>
                (x.id == id)
            )
        {
            AddInclude(x => x.UserProfile);
        }
    }
}
