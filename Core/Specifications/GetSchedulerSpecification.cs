using Core.Entities;

namespace Core.Specifications
{
    public class GetSchedulerSpecification: BaseSpecification<Scheduler>
    {
        public GetSchedulerSpecification(CommonSpecParams specParams, int campaignId, int userProfileId)
            :base(x => 
                (x.UserProfileId == userProfileId) &&
                (x.CampaignId == campaignId) &&
                (
                    (x.FirstName.Contains(specParams.Search)) ||
                    (x.LastName.Contains(specParams.Search)) ||
                    (x.Company.Contains(specParams.Search)) ||
                    (string.IsNullOrEmpty(specParams.Search))
                )
            )
        {
            AddOrderBy(x => x.FirstName);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.FirstName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.FirstName);
                        break;
                    default:
                        AddOrderBy(p => p.FirstName);
                        break;
                }
            }
        }
    }
}
