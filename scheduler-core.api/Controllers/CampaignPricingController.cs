using AutoMapper;
using Core.Dtos.CampaignPricings.Output;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class CampaignPricingController : BaseApiController
    {
        private readonly ICampaignPricingService _campaignPricingService;
        private readonly IMapper _mapper;
        public CampaignPricingController(IMapper mapper, ICampaignPricingService campaignPricingService)
        {
            _campaignPricingService = campaignPricingService;
            _mapper = mapper;
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
    }
}
