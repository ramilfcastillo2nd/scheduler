using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignsSpecification: BaseSpecification<Campaign>
    {
        public GetCampaignsSpecification(CommonSpecParams specParams)
            : base(x =>
                 ((x.CampaignName.ToLower().Contains(specParams.Search)) ||
                 (x.Plan.ToLower().Contains(specParams.Search)) ||
                 (x.Client.ToLower().Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search)
            )
        {
            AddInclude(x => x.CampaignAssignments);
            AddOrderBy(x => x.CampaignName);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.CampaignName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.CampaignName);
                        break;
                    default:
                        AddOrderBy(p => p.CampaignName);
                        break;
                }
            }
        }
    }
}
