using Core.Dtos.CampaignPricings.Output;
using Core.Interfaces;
using Core.Entities.Identity;
using Core.Entities;
using Core.Dtos.CampaignPricings.Input;
using scheduler_core.api.Errors;
using scheduler_core.api.Extensions;

using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace scheduler_core.api.Controllers
{
    public class CampaignPricingController : BaseApiController
    {
        private readonly ICampaignPricingService _campaignPricingService;
        private readonly IMapper _mapper;

        private readonly UserManager<AppUser> _userManager;

        public CampaignPricingController(
            IMapper mapper,
            ICampaignPricingService campaignPricingService,
            UserManager<AppUser> userManager)
        {
            _campaignPricingService = campaignPricingService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCampaignPricing([FromBody] CreateCampaignPricingInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var validationResponse = await _campaignPricingService.ValidateCreateInput(request);

                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var campaignPricingMapped = _mapper.Map<CampaignPricing>(request);
                campaignPricingMapped.CreatedDate = DateTime.UtcNow;
                campaignPricingMapped.CreatedBy = user.Id.ToString();

                await _campaignPricingService.CreateCampaignPricing(campaignPricingMapped);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCampaignPricing()
        {
            try
            {
                var campaignPricings = await _campaignPricingService.GetCampaignPricings();
                var campaignPricingMapped = _mapper.Map<IReadOnlyList<GetCampaignPricingOutputDto>>(campaignPricings);
                return Ok(campaignPricingMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaignById(int id)
        {
            try
            {
                var campaignPricing = await _campaignPricingService.GetCampaignPricingById(id);
                var campaignPricingMapped = _mapper.Map<GetCampaignPricingOutputDto>(campaignPricing);
                return Ok(campaignPricingMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCampaignPrice([FromBody] UpdateCampaignPricingInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);

                var campaignPrice = await _campaignPricingService.GetCampaignPricingById(request.Id);
                if (campaignPrice == null)
                    return BadRequest(new ApiResponse(400, "Campaign Price is not existing."));

                var validationResponse = await _campaignPricingService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                campaignPrice.id = request.Id;
                campaignPrice.Name = request.Name;
                campaignPrice.Pricing = request.Pricing;
                campaignPrice.DuarationId = request.DuarationId;
                campaignPrice.UpdatedBy = user.Id.ToString();
                campaignPrice.UpdatedDate = DateTime.UtcNow;

                await _campaignPricingService.UpdateCampaignPrice(campaignPrice);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "superadmin, admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaignPrice(int id)
        {
            try
            {
                var campaignPrice = await _campaignPricingService.GetCampaignPricingById(id);
                if (campaignPrice == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                await _campaignPricingService.DeleteCampaignPrice(campaignPrice);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
