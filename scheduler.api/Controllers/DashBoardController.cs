using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scheduler.api.Errors;
using System.Threading.Tasks;

namespace scheduler.api.Controllers
{
    public class DashBoardController : BaseApiController
    {
        private readonly IDashBoardService _dashBoardService;
        public DashBoardController(IDashBoardService dashBoardService)
        {
            _dashBoardService = dashBoardService;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetDashBoard()
        {
            try
            {
                var dashBoardOutput = await _dashBoardService.GetDashBoardCount();
                return Ok(dashBoardOutput);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
