using Core.Dtos.Campaigns.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Enums;
using Core.Helpers;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CampaignService: ICampaignService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public CampaignService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<int> GetCampaignsCount(CommonSpecParams specParams)
        {
            var specs = new GetCampaignsCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Campaign>().CountAsync(specs);
            return count;
        }

        public async Task<IReadOnlyList<Campaign>> GetCampaigns(CommonSpecParams specParams)
        {
            var specs = new GetCampaignsSpecification(specParams);
            var campaigns = await _unitOfWork.Repository<Campaign>().ListAsync(specs);
            return campaigns;
        }

        public async Task ChangeStatus(ChangeCampaignStatusInputDto request)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
            campaign.StatusId = request.StatusId;
            _unitOfWork.Repository<Campaign>().Update(campaign);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignStatusInputDto request)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
            if(campaign ==  null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign is not existing.",
                    StatusCode = 400
                };

            //Check if status is valid
            var exists = Enum.IsDefined(typeof(CampaignStatus), request.StatusId);
            if (!exists)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "StatusId is not valid.",
                    StatusCode = 400
                };


            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task<ValidationOutputDto> ValidateCreateInput(CreateCampaignInputDto request)
        {
            if(string.IsNullOrEmpty(request.CampaignName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Name is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Plan))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Plan is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Client))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client is a required field.",
                    StatusCode = 400 
                };

            if (string.IsNullOrEmpty(request.ClientEmail))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client Email is a required field.",
                    StatusCode = 400
                };

            var isValidEmail = RegexUtilities.IsValidEmail(request.ClientEmail);

            if (!isValidEmail)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client Email is a not valid.",
                    StatusCode = 400
                };

            //Check if status is valid
            var exists = Enum.IsDefined(typeof(CampaignStatus), request.StatusId);
            if (!exists)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "StatusId is not valid.",
                    StatusCode = 400
                };

            return new ValidationOutputDto { 
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task CreateCampaign(Campaign request)
        {
            _unitOfWork.Repository<Campaign>().Add(request);
            await _unitOfWork.Complete();
        }

        public async Task<Campaign> GetCampaignById(int id)
        {
            var spec = new GetCampaignByIdWithDetailsSpecification(id);
            var campaign = await _unitOfWork.Repository<Campaign>().ListAsync(spec);
            return campaign.FirstOrDefault();
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateCampaignInputDto request)
        {
            if (string.IsNullOrEmpty(request.CampaignName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Name is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Plan))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Plan is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Client))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.ClientEmail))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client Email is a required field.",
                    StatusCode = 400
                };

            var isValidEmail = RegexUtilities.IsValidEmail(request.ClientEmail);

            if (!isValidEmail)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Client Email is a not valid.",
                    StatusCode = 400
                };
            //Check if status is valid
            var exists = Enum.IsDefined(typeof(CampaignStatus), request.StatusId);
            if (!exists)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "StatusId is not valid.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task UpdateCampaign(Campaign request)
        {
            _unitOfWork.Repository<Campaign>().Update(request);
            await _unitOfWork.Complete();
        }

        public async Task DeleteCampaign(Campaign campaign)
        {
            _unitOfWork.Repository<Campaign>().Delete(campaign);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateGetCampaignsByUserInput(int userProfileId)
        {
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(userProfileId);

            if (userProfile == null)
                return new ValidationOutputDto { 
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User does not exists."
                };

            var campaignAssignmentsSpecs = new GetCampaignAssignmentListByUserProfileIdSpecification(userProfileId);
            var campaignAssigments = await _unitOfWork.Repository<CampaignAssignment>().ListAsync(campaignAssignmentsSpecs);
            if (campaignAssigments == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "There is no campaign assignment for this user."
                };

            if(campaignAssigments.Count <= 0)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "There is no campaign assignment for this user."
                };

            return new ValidationOutputDto {
                IsSuccess = true,
                StatusCode = 200,
                Message = string.Empty
            };
        }

        public async Task<ValidationOutputDto> ValidateGetCampaignsByManagerInput(int userProfileId)
        {
            var userProfileSpecs = new GetUserProfileByIdWithDetailsSpecification(userProfileId);
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(userProfileSpecs);

            if (userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User does not exists."
                };

            var appUser = userProfile.AppUser;
            var role = await _userManager.GetRolesAsync(appUser);

            if (role.First() != "manager")
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User is not a manager"
                };

            var campaignsSpecs = new GetCampaignsByManagerIdSpecification(userProfileId);
            var campaignAssigments = await _unitOfWork.Repository<Campaign>().ListAsync(campaignsSpecs);
            if (campaignAssigments == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false, 
                    StatusCode = 400,
                    Message = "There are no campaigns assiged to this manager"
                };

            if (campaignAssigments.Count <= 0)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "There are no campaigns assiged to this manager"
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = string.Empty
            };
        }

        public async Task<IReadOnlyList<Campaign>> GetCampaignsByUser(int userProfileId)
        {
            var campaignAssignmentsSpecs = new GetCampaignAssignmentListByUserProfileIdSpecification(userProfileId);
            var campaignAssigments = await _unitOfWork.Repository<CampaignAssignment>().ListAsync(campaignAssignmentsSpecs);
            var campaigns = campaignAssigments.Select(s => s.Campaign).Distinct().ToList();
            return campaigns;
        }

        public async Task<IReadOnlyList<Campaign>> GetCampaignsByManager(int userProfileId)
        {
            var campaignsSpecs = new GetCampaignsByManagerIdSpecification(userProfileId);
            var campaigns = await _unitOfWork.Repository<Campaign>().ListAsync(campaignsSpecs);
            return campaigns;
        }
    }
}
