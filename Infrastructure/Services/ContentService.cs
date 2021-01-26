using Core.Dtos.Contents.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class ContentService: IContentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Content> GetContentByCampaignId(int campaignId)
        {
            var spec = new GetContentByCampaignIdSpecification(campaignId);
            var content = await _unitOfWork.Repository<Content>().GetEntityWithSpec(spec);
            return content;
        }

        public async Task SaveContent(SaveContentInputDto request, string userId)
        {
            var getContentSpecs = new GetContentByCampaignIdSpecification(request.CampaignId);
            var content = await _unitOfWork.Repository<Content>().GetEntityWithSpec(getContentSpecs);
            if (content == null)
            {
                var contentRequest = new Content
                {
                    InviteMessage = request.InviteMessage,
                    Message1 = request.Message1,
                    Message2 = request.Message2,
                    Message3 = request.Message3,
                    Message4 = request.Message4,
                    Message5 = request.Message5,
                    CampaignId = request.CampaignId,
                    CreatedBy = userId,
                    CreatedDate = DateTime.UtcNow
                };

                _unitOfWork.Repository<Content>().Add(contentRequest);
                await _unitOfWork.Complete();
            }
            else
            {
                content.InviteMessage = request.InviteMessage;
                content.Message1 = request.Message1;
                content.Message2 = request.Message2;
                content.Message3 = request.Message3;
                content.Message4 = request.Message4;
                content.Message5 = request.Message5;
                content.UpdatedBy = userId;
                content.UpdatedDate = DateTime.UtcNow;

                _unitOfWork.Repository<Content>().Update(content);
                await _unitOfWork.Complete();
            }
        }

        public async Task<ValidationOutputDto> ValidateGetInput(int campaignId)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(campaignId);
            if (campaign == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Campaign is not existing."
                };
            var specs = new GetContentByCampaignIdSpecification(campaignId);
            var content = await _unitOfWork.Repository<Content>().GetEntityWithSpec(specs);
            if (content == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Campaign has no content."
                };

            return new ValidationOutputDto
            {
                Message = string.Empty,
                StatusCode = 200,
                IsSuccess = true
            };
        }

        public async Task<ValidationOutputDto> ValidateSaveInput(SaveContentInputDto request)
        {
            var campaign = await _unitOfWork.Repository<Campaign>().GetByIdAsync(request.CampaignId);
            if (campaign == null)
                return new ValidationOutputDto { 
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Campaign is not existing."
                };

            return new ValidationOutputDto { 
                Message = string.Empty,
                StatusCode = 200,
                IsSuccess = true
            };
        }
    }
}
