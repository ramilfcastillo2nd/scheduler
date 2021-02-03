using Core.Entities;

namespace Core.Specifications
{
    public class GetWagesSpecification : BaseSpecification<Wage>
    {
        public GetWagesSpecification(CommonSpecParams specParams)
            : base()
        {
            AddOrderBy(x => x.CampaignType);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.CampaignType);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.CampaignType);
                        break;
                    default:
                        AddOrderBy(p => p.CampaignType);
                        break;
                }
            }
        }
    }
}
