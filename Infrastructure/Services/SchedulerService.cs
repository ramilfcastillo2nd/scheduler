using Core.Dtos.Schedulers.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SchedulerService: ISchedulerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserProfileService _userProfileService;
        private readonly ICampaignAssignmentService _campaignAssignmentService;
        private readonly IContentService _contentService;
        public SchedulerService(IContentService contentService, ICampaignAssignmentService campaignAssignmentService, IUserProfileService userProfileService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userProfileService = userProfileService;
            _campaignAssignmentService = campaignAssignmentService;
            _contentService = contentService;
        }

        public async Task<(int, IReadOnlyList<Scheduler>)> GetSchedulers(int campaignId, int userProfileId, CommonSpecParams specParams)
        {
            var specGetScheduler = new GetSchedulerSpecification(specParams, campaignId, userProfileId);
            var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(specGetScheduler);
            var specSchedulerCount = new GetSchedulerCountSpecification(specParams, campaignId, userProfileId);
            var count = await _unitOfWork.Repository<Scheduler>().CountAsync(specSchedulerCount);
            return (count, schedulers);
        }

        public async Task InsertScheduler(List<Scheduler> schedulers, UploadSchedulerInputDto request, string userId)
        {
            schedulers = schedulers.Select(s =>
            {
                s.UserProfileId = request.UserProfileId;
                s.CampaignId = request.CampaignId;
                s.CreatedBy = userId;
                s.CreatedDate = DateTime.UtcNow;
                return s;
            }).ToList();

            _unitOfWork.Repository<Scheduler>().AddRange(schedulers);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateUploadInput(UploadSchedulerInputDto request)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
            if (campaign == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Campaign is not existing."
                };

            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.UserProfileId);
            if (userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User Profile is not existing."
                };

            var ext = Path.GetExtension(request.CsvData.FileName);
            if (ext != ".csv")
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Uploaded file is not type of CSV."
                };

            if (request.CsvData.Length <= 0)
            {
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "File has no content"
                };
            }

            return new ValidationOutputDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = string.Empty
            };
        }

        public async Task<Scheduler> GetSchedulerById(int id)
        {
            var scheduler = await _unitOfWork.Repository<Scheduler>().GetByIdAsync(id);
            return scheduler;
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateSchedulerInputDto request)
        {
            if (string.IsNullOrEmpty(request.FirstName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "FirstName is a required field."
                };

            if (string.IsNullOrEmpty(request.LastName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "LastName is a required field."
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = string.Empty
            };
        }

        public async Task<ValidationOutputDto> ValidateGetInput(int campaignId, int userProfileId)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(campaignId);
            if (campaign == null)
                return new ValidationOutputDto { 
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Campaign is not existing."
                };

            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(userProfileId);
            if (userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "User Profile is not existing."
                };
      
            return new ValidationOutputDto
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = string.Empty
            };
        }

        public async Task UpdateScheduler(Scheduler scheduler)
        {
            _unitOfWork.Repository<Scheduler>().Update(scheduler);
            await _unitOfWork.Complete();
        }

        public async Task<(int, IReadOnlyList<Scheduler>, Campaign, Content)> GetSchedulers(Guid userId, CommonSpecParams specParams)
        {
            var userProfile = await _userProfileService.GetUserProfileByUserId(userId);
            var campaignAssignment = await _campaignAssignmentService.GetActiveCampaignAssignment(userProfile.id);

            if (campaignAssignment == null) return (0, null, null, null);

            var specGetScheduler = new GetSchedulerSpecification(specParams, campaignAssignment.CampaignId, campaignAssignment.UserProfileId);
            var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(specGetScheduler);
            var campaign = campaignAssignment.Campaign;
            var content = await _contentService.GetContentByCampaignId(campaign.id);
            var specSchedulerCount = new GetSchedulerCountSpecification(specParams, campaignAssignment.CampaignId, campaignAssignment.UserProfileId);
            var count = await _unitOfWork.Repository<Scheduler>().CountAsync(specSchedulerCount);
            return (count, schedulers, campaign, content);
        }
    }
}
