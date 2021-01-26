using AutoMapper;
using Core.Dtos.Contents.Input;
using Core.Dtos.Contents.Output;
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
    public class ContentController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IContentService _contentService;
        private readonly IMapper _mapper;
        public ContentController(IMapper mapper, IContentService contentService, UserManager<AppUser> userManager)
        {
            _contentService = contentService;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SaveOrUpdate([FromBody]SaveContentInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var validationResponse = await _contentService.ValidateSaveInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _contentService.SaveContent(request, user.Id.ToString());
                return Ok(new  ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong"));
            }
        }

        [Authorize]
        [HttpGet("campaignid={campaignId}")]
        public async Task<IActionResult> GetContentByCampaignId(int campaignId)
        {
            try
            {
                var validationResponse = await _contentService.ValidateGetInput(campaignId);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var content = await _contentService.GetContentByCampaignId(campaignId);
                var contentMapped = _mapper.Map<GetContentOutputDto>(content);
                return Ok(contentMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong"));
            }
        }
    }
}
