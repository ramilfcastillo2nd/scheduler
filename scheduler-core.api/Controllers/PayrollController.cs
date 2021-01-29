using AutoMapper;
using Core.Dtos.Payrolls;
using Core.Dtos.Payrolls.Input;
using Core.Dtos.Payrolls.Output;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using scheduler_core.api.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class PayrollController : BaseApiController
    {
        private readonly IPayrollService _payrollService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PayrollController(IMapper mapper, UserManager<AppUser> userManager, IPayrollService payrollService)
        {
            _payrollService = payrollService;
            _userManager = userManager;
            _mapper = mapper;
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

        [HttpGet("currentsdr")]
        public async Task<IActionResult> GetPayrollByEmployeeId()
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var payroll = await _payrollService.GetPayrollByUserProfileId(user.Id);
                var payrollMapped = _mapper.Map<IReadOnlyList<GetPayrollOutputDto>>(payroll);
                return Ok(payrollMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
