using Core.Dtos.Campaigns.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CampaignAssignmentService: ICampaignAssignmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CampaignAssignmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task ChangeStatus(ChangeCampaignSdrStatusInputDto request)
        {
            var specs = new GetExistingCampaignAssignmentSpecification(request.CampaignId, request.UserProfileId);
            var campaignAssignment = await _unitOfWork.Repository<CampaignAssignment>().GetEntityWithSpec(specs);
            campaignAssignment.StatusId = request.StatusId;

            //Update
            _unitOfWork.Repository<CampaignAssignment>().Update(campaignAssignment);
            await _unitOfWork.Complete();
        }

        public async Task CreateCampaignAssignment(CampaignAssignment request)
        {
            _unitOfWork.Repository<CampaignAssignment>().Add(request);
            await _unitOfWork.Complete();
        }

        public async Task DeleteAssignment(CampaignAssignment campaignAssignment)
        {
            _unitOfWork.Repository<CampaignAssignment>().Delete(campaignAssignment);
            await _unitOfWork.Complete();
        }

        public async Task<CampaignAssignment> GetActiveCampaignAssignment(int userProfileId)
        {
            var specs = new GetCampaignAssignmentByUserProfileIdSpecification(userProfileId);
            var campaignAssignments = await _unitOfWork.Repository<CampaignAssignment>().ListAsync(specs);

            if (campaignAssignments == null)
                return null;

            return campaignAssignments.FirstOrDefault();
        }

        public async Task<CampaignAssignment> GetExistingCampaignAssignment(int campaignId, int userProfileId)
        {
            var specs = new GetExistingCampaignAssignmentSpecification(campaignId, userProfileId);
            var campaignAssignment = (await _unitOfWork.Repository<CampaignAssignment>().ListAsync(specs)).FirstOrDefault();
            return campaignAssignment;
        }

        public async Task<(IReadOnlyList<UserProfile>, int)> GetSdrsFromCampaign(int campaignId, CommonSpecParams specParams)
        {
            var specs = new GetCampaignSdrsSpecification(campaignId);
            var campaignAssignments = await _unitOfWork.Repository<CampaignAssignment>().ListAsync(specs);
            var skip = specParams.PageSize * (specParams.PageIndex - 1);
            var take = specParams.PageSize;
            var sdrs = campaignAssignments.Select(s => {
                s.UserProfile.StatusId = s.StatusId;
                return s.UserProfile;
                }).ToList();

            var count = sdrs.Count();
            specParams.Search = specParams.Search == null ? string.Empty : specParams.Search;
            sdrs = sdrs.Where(s =>
                (
                    s.FirstName.Contains(specParams.Search) ||
                    s.LastName.Contains(specParams.Search) ||
                    s.LinkedInName.Contains(specParams.Search) ||
                    s.Email.Contains(specParams.Search)
                ) || string.IsNullOrEmpty(specParams.Search)
            ).Skip(skip).Take(take).ToList();
            return (sdrs, count);
        }

        public async Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignSdrStatusInputDto request)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
            if (campaign == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign is not existing.",
                    StatusCode = 400
                };

            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.UserProfileId);
            if (userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "User is not existing.",
                    StatusCode = 400
                };

            //Check if status is valid
            var exists = Enum.IsDefined(typeof(AssignedSdrStatus), request.StatusId);
            if (!exists)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "StatusId is not valid.",
                    StatusCode = 400
                };

            var specs = new GetExistingCampaignAssignmentSpecification(request.CampaignId, request.UserProfileId);
            var campaignAssignment = await _unitOfWork.Repository<CampaignAssignment>().GetEntityWithSpec(specs);
            
            if(campaignAssignment == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "User is not assigned to this campaign.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }


    }
}
