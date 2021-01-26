using Core.Dtos.Dashboards.Output;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDashBoardService
    {
        Task<GetDashboardStatsOutputDto> GetDashBoardCount();
    }
}
