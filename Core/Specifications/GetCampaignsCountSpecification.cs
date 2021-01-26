using Core.Entities;

namespace Core.Specifications
{
    public class GetCampaignsCountSpecification : BaseSpecification<Campaign>
    {
        public GetCampaignsCountSpecification(CommonSpecParams specParams)
            : base(x =>
                 ((x.CampaignName.ToLower().Contains(specParams.Search)) ||
                 (x.Plan.ToLower().Contains(specParams.Search)) ||
                 (x.Client.ToLower().Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search)
            )
        {
           
        }
    }
}
