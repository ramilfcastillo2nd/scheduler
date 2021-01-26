using Core.Entities;
using System.Linq;

namespace Core.Specifications
{
    public class GetSchedulersByCampaignIdsSpecification: BaseSpecification<Scheduler>
    {
        public GetSchedulersByCampaignIdsSpecification(int[] campaignIds)
            : base(x =>
                (campaignIds.Contains(x.CampaignId.Value))
            )
        {

        }
    }
}
