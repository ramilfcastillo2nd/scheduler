using Core.Dtos.Dashboards.Output;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DashBoardService : IDashBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashBoardService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<GetDashboardStatsOutputDto> GetDashBoardCount()
        {
            var activeCampaignsSpecs = new GetActiveCampaignsSpecification();
            var activeCampaigns = await _unitOfWork.Repository<Campaign>().ListAsync(activeCampaignsSpecs);

            if (activeCampaigns != null)
            {
                if (activeCampaigns.Count > 0)
                {
                    var campaignIds = activeCampaigns.Select(s => s.id).ToArray();
                    var schedulersSpec = new GetSchedulersByCampaignIdsSpecification(campaignIds);
                    var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(schedulersSpec);

                    return new GetDashboardStatsOutputDto
                    {
                        Invites = schedulers.Where(s => s.ConnectionRequest != null || s.ConnectionRequest == true).Count(),
                        Views = schedulers
                                .Where(s => s.ConnectionRequest != null || s.ConnectionRequest == false).Count(),
                        Messages = schedulers
                                .Where(s => 
                                        (s.IsMessage1 == true || s.IsMessage1 != null) ||
                                        (s.IsMessage2 == true || s.IsMessage2 != null) ||
                                        (s.IsMessage3 == true || s.IsMessage3 != null) ||
                                        (s.IsMessage4 == true || s.IsMessage4 != null) ||
                                        (s.IsMessage5 == true || s.IsMessage5 != null)
                                    ).Count(),
                        InMails = schedulers.Where(s => s.ConnectionRequest != null || s.ConnectionRequest == true).Count(),
                        Emails = schedulers.Where(s => s.ConnectionRequest != null || s.ConnectionRequest == true).Count(),
                        Connected = schedulers.Where(s => s.RequestAccepted != null || s.RequestAccepted == true).Count(),
                        FollowUps = 0,
                        Greetings = 0,
                        Withdrawals = 0
                    };
                }

                return new GetDashboardStatsOutputDto
                {
                    Invites = 0,
                    Views = 0,
                    Messages = 0,
                    InMails = 0,
                    Emails = 0,
                    Connected = 0,
                    FollowUps = 0,
                    Greetings = 0,
                    Withdrawals = 0
                };
            }
            else {
                return new GetDashboardStatsOutputDto
                {
                    Invites = 0,
                    Views = 0,
                    Messages = 0,
                    InMails = 0,
                    Emails = 0,
                    Connected = 0,
                    FollowUps = 0,
                    Greetings = 0,
                    Withdrawals = 0
                };
            }
        }
    }
}
