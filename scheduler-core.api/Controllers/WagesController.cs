using Core.Interfaces;
using Core.Entities.Identity;
using Core.Entities;
using Core.Dtos.Wages.Input;
using Core.Dtos.Wages.Output;
using Core.Specifications;
using scheduler_core.api.Helpers;
using scheduler_core.api.Extensions;
using scheduler_core.api.Errors;

using AutoMapper;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

namespace scheduler_core.api.Controllers
{
    public class WagesController : BaseApiController
    {
        private readonly IWageService _wageService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public WagesController(
            IWageService wageService,
            IMapper mapper,
            UserManager<AppUser> userManager)
        {
            _wageService = wageService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpPost]
        public async Task<IActionResult> AddWageAsync([FromBody] AddWageInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var validationResponse = await _wageService.ValidateCreateInputAsync(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var wageMapped = _mapper.Map<Wage>(request);
                await _wageService.CreateWage(wageMapped);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<GetWageOutputDto>>> GetAllWages([FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var wages = await _wageService.GetWagesAsync(specParams);
                var departmentsMapped = _mapper.Map<IReadOnlyList<GetWageOutputDto>>(wages);

                return Ok(new Pagination<GetWageOutputDto>(specParams.PageIndex, specParams.PageSize, 0, departmentsMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWageAsync(int id)
        {
            try
            {
                //Validate Input
                var wage = await _wageService.GetWageInfoByIdAsync(id);
                if (wage == null)
                    return BadRequest(new ApiResponse(400, "Wage info is not existing."));

                await _wageService.DeleteWage(wage);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpPut]
        public async Task<ActionResult<GetWageOutputDto>> UpdateWageAsync([FromBody] UpdateWageInputDto request)
        {
            try
            {
                var wage = await _wageService.GetWageInfoByIdAsync(request.Id);
                if (wage == null) return BadRequest(new ApiResponse(400, "Wage info not existing."));

                var validationResponse = await _wageService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess) return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                wage.SdrType = request.SdrType;
                wage.CampaignType = request.CampaignType;
                wage.BasePay = request.BasePay;
                wage.IncentivePay = request.IncentivePay;

                await _wageService.UpdateWage(wage);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
