using AutoMapper;
using Core.Dtos.SdrGroupings.Input;
using Core.Dtos.UserProfiles.Output;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler.api.Errors;
using scheduler_user.api.Extensions;
using scheduler_user.api.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace scheduler_user.api.Controllers
{
    public class SdrGroupingsController : BaseApiController
    {
        private readonly ISdrGroupingService _sdrGroupingService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public SdrGroupingsController(IMapper mapper, UserManager<AppUser> userManager, ISdrGroupingService sdrGroupingService)
        {
            _sdrGroupingService = sdrGroupingService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> AssignSdrToManager([FromBody] AssignSdrToManagerInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

                var validationResponse = await _sdrGroupingService.ValidateAssignSdrToManagerInput(request);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _sdrGroupingService.AssignSdrToManager(request, user.Id.ToString());

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [HttpGet("managerId={managerId}")]
        public async Task<IActionResult> GetSdrsUnderManager(int managerId, [FromQuery]CommonSpecParams specParams)
        {
            try
            {
                var validationResponse = await _sdrGroupingService.ValidateManagerIdGetInput(managerId);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var sdrGroupingDetails = await _sdrGroupingService.GetSdrGroupingByManageId(managerId, specParams);
                var managerMapped = _mapper.Map<GetUserProfileOutputDto>(sdrGroupingDetails.Item1);
                var sdrListMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(sdrGroupingDetails.Item2);
                var count = sdrGroupingDetails.Item3;
                return Ok(new { 
                   Manager = managerMapped,
                   Sdrs = new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, sdrListMapped)
                });
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
