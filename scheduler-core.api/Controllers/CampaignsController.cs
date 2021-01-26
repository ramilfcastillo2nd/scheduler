using AutoMapper;
using Core.Dtos.Campaigns.Input;
using Core.Dtos.Campaigns.Output;
using Core.Dtos.UserProfiles.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using scheduler_core.api.Extensions;
using scheduler_core.api.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class CampaignsController : BaseApiController
    {
        private readonly ICampaignAssignmentService _campaignAssignmentService;
        private readonly ICampaignService _campaignService;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserProfileService _userProfileService;
        public CampaignsController(ICampaignAssignmentService campaignAssignmentService, IUserProfileService userProfileService, UserManager<AppUser> userManager, IMapper mapper, ICampaignService campaignService)
        {
            _campaignService = campaignService;
            _mapper = mapper;
            _userManager = userManager;
            _userProfileService = userProfileService;
            _campaignAssignmentService = campaignAssignmentService;
        }

        [Authorize]
        [HttpGet("employeeid={employeeId}")]
        public async Task<IActionResult> GetCampaignsByUser(int employeeId)
        {
            try
            {

                var validationResponse = await _campaignService.ValidateGetCampaignsByUserInput(employeeId);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var campaigns = await _campaignService.GetCampaignsByUser(employeeId);
                var campaignsMapped = _mapper.Map<IReadOnlyList<GetCampaignOutputDto>>(campaigns);
                return Ok(campaignsMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }


        [Authorize]
        [HttpGet("managerid={employeeId}")]
        public async Task<IActionResult> GetCampaignsByManagerId(int employeeId)
        {
            try
            {
                var validationResponse = await _campaignService.ValidateGetCampaignsByManagerInput(employeeId);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var campaigns = await _campaignService.GetCampaignsByManager(employeeId);
                var campaignsMapped = _mapper.Map<IReadOnlyList<GetCampaignOutputDto>>(campaigns);
                return Ok(campaignsMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> CreateCampaign([FromBody] CreateCampaignInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var validationResponse = await _campaignService.ValidateCreateInput(request);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message)); 

                var campaignMapped = _mapper.Map<Campaign>(request);
                campaignMapped.CreatedDate = DateTime.UtcNow;
                campaignMapped.CreatedBy = user.Id.ToString();
                await _campaignService.CreateCampaign(campaignMapped);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetCampaigns([FromQuery]CommonSpecParams specParams)
        {
            try
            {
                var campaigns = await _campaignService.GetCampaigns(specParams);
                var campaignsMapped = _mapper.Map<IReadOnlyList<GetCampaignOutputDto>>(campaigns);
                var count = await _campaignService.GetCampaignsCount(specParams);
                return Ok(new Pagination<GetCampaignOutputDto>(specParams.PageIndex, specParams.PageSize, count, campaignsMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet("currentuser")]
        public async Task<IActionResult> GetCampaignsCurrentUser()
        {
            try
            {
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var profile = await _userProfileService.GetUserProfileByUserId(user.Id);
                var validationResponse = await _campaignService.ValidateGetCampaignsByUserInput(profile.id);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var campaigns = await _campaignService.GetCampaignsByUser(profile.id);
                var campaignsMapped = _mapper.Map<IReadOnlyList<GetCampaignOutputDto>>(campaigns);
                return Ok(campaignsMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCampaign(int id)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(id);
                if(campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));
                var campaignMapped = _mapper.Map<GetCampaignDetailOutputDto>(campaign);
                return Ok(campaignMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("statuschange")]
        public async Task<IActionResult> ChangeCampaignStatus([FromBody] ChangeCampaignStatusInputDto request)
        {
            try
            {
                var validationResponse = await _campaignService.ValidateChangeStatusInput(request);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _campaignService.ChangeStatus(request);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("sdr/statuschange")]
        public async Task<IActionResult> ChangeAssignedSdrStatus([FromBody] ChangeCampaignSdrStatusInputDto request)
        {
            try
            {
                var validationResponse = await _campaignAssignmentService.ValidateChangeStatusInput(request);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _campaignAssignmentService.ChangeStatus(request);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCampaign([FromBody] UpdateCampaignInputDto request)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(request.id);
                if(campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                var validationResponse = await _campaignService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                campaign.CampaignName = request.CampaignName;
                campaign.Client = request.Client;
                campaign.Plan = request.Plan;
                campaign.Started = request.Started;
                campaign.StatusId = request.StatusId;
                campaign.ClientEmail = request.ClientEmail;
                campaign.TrelloEditorId = request.TrelloEditorId;
                campaign.AccountsExecutiveId = request.AccountsExecutiveId;

                await _campaignService.UpdateCampaign(campaign);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("assignmanager")]
        public async Task<IActionResult> AssignManagerToCampaign([FromBody] AssignManagerToCampaignInputDto request)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(request.CampaignId);
                if (campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                var userProfile = await _userProfileService.GetUserProfileById(request.UserProfileId);
                if (userProfile == null)
                    return BadRequest(new ApiResponse(400, "User Profile is not existing."));

                var user = await _userManager.FindByIdAsync(userProfile.UserId.ToString());
                var role = (await _userManager.GetRolesAsync(user)).First();

                if (role.ToLower() != "manager")
                    return BadRequest(new ApiResponse(400, "User is not assigned as Manager."));

                campaign.UserProfileId = request.UserProfileId;

                await _campaignService.UpdateCampaign(campaign);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPut("assignsdr")]
        public async Task<IActionResult> AssignSdrToCampaign([FromBody] AssignSdrToCampaignInputDto request)
        {
            try
            {
                var currentUser = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var campaign = await _campaignService.GetCampaignById(request.CampaignId);
                if (campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                var userProfile = await _userProfileService.GetUserProfileById(request.UserProfileId);
                if (userProfile == null)
                    return BadRequest(new ApiResponse(400, "User Profile is not existing."));

                var user = await _userManager.FindByIdAsync(userProfile.UserId.ToString());
                var role = (await _userManager.GetRolesAsync(user)).First();

                if(role.ToLower() != "sdr")
                    return BadRequest(new ApiResponse(400, "User is not assigned as SDR."));

                var existingCampaignAssignment = await _campaignAssignmentService.GetExistingCampaignAssignment(request.CampaignId, request.UserProfileId);
                if (existingCampaignAssignment != null)
                    return BadRequest(new ApiResponse(400, "SDR is already assigned to this campaign."));

                var campaignAssignment = new CampaignAssignment { 
                    CampaignId = request.CampaignId,
                    UserProfileId = request.UserProfileId,
                    CreatedDate = DateTime.UtcNow,
                    CreatedBy = currentUser.Id.ToString()
                };

                await _campaignAssignmentService.CreateCampaignAssignment(campaignAssignment);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet("assignedsdr/campaignId={campaignId}")]
        public async Task<IActionResult> GetAllSdrAssignedToCampaign(int campaignId, [FromQuery]CommonSpecParams specParams)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(campaignId);
                if (campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));
               
                var users = await _campaignAssignmentService.GetSdrsFromCampaign(campaignId, specParams);
                var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(users.Item1);
                var count = users.Item2;
                return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
            }
            catch(Exception x)
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCampaign(int id)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(id);
                if (campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                await _campaignService.DeleteCampaign(campaign);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("unassignsdr/{campaignId}/{userProfileId}")]
        public async Task<IActionResult> DeleteSdrFromCampaign(int campaignId, int userProfileId)
        {
            try
            {
                var campaign = await _campaignService.GetCampaignById(campaignId);
                if (campaign == null)
                    return BadRequest(new ApiResponse(400, "Campaign is not existing."));

                var userProfile = await _userProfileService.GetUserProfileById(userProfileId);
                if (userProfile == null)
                    return BadRequest(new ApiResponse(400, "User Profile is not existing."));

                var campaignAssignment = await _campaignAssignmentService.GetExistingCampaignAssignment(campaignId, userProfileId);
                if(campaignAssignment == null)
                    return BadRequest(new ApiResponse(400, "There is no current assignment for user."));

                await _campaignAssignmentService.DeleteAssignment(campaignAssignment);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
