using Core.Dtos.Payrolls;
using Core.Dtos.Payrolls.Input;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using scheduler_core.api.Extensions;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class PayrollController : BaseApiController
    {
        private readonly IPayrollService _payrollService;
        private readonly UserManager<AppUser> _userManager;
        public PayrollController(UserManager<AppUser> userManager, IPayrollService payrollService)
        {
            _payrollService = payrollService;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        [HttpPost("process")]
        public async Task<IActionResult> ProcessPayroll([FromBody] ProcessPayrollInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                await _payrollService.ProcessPayrollPeriod(request, user.Id);
                return Ok();
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        public async Task<IActionResult> GetPayrollByEmployeeId(int employeeId)
        {
            try
            {
                return Ok();
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
