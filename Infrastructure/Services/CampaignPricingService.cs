using Core.Dtos.CampaignPricings.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Interfaces;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CampaignPricingService: ICampaignPricingService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CampaignPricingService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCampaignPricing(CampaignPricing request)
        {
            _unitOfWork.Repository<CampaignPricing>().Add(request);
            await _unitOfWork.Complete();
        }

        public async Task<CampaignPricing> GetCampaignPricingById(int id)
        {
            var campaignPricing = await _unitOfWork.Repository<CampaignPricing>().GetByIdAsync(id);
            return campaignPricing;
        }

        public async Task<IReadOnlyList<CampaignPricing>> GetCampaignPricings()
        {
            var campaignPricings = await _unitOfWork.Repository<CampaignPricing>().ListAllAsync();
            return campaignPricings;
        }

        public async Task UpdateCampaignPrice(CampaignPricing request)
        {
            _unitOfWork.Repository<CampaignPricing>().Update(request);
            await _unitOfWork.Complete();
        }

        public async Task DeleteCampaignPrice(CampaignPricing campaignPrice)
        {
            _unitOfWork.Repository<CampaignPricing>().Delete(campaignPrice);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateCreateInput(CreateCampaignPricingInputDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price Name is a required field.",
                    StatusCode = 400
                };

            if ((decimal)request.Pricing == 0 || request.Pricing == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price is a required field.",
                    StatusCode = 400
                };

            if ((int)request.DuarationId == 0 || request.DuarationId == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price Duration is a required field.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateCampaignPricingInputDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price Name is a required field.",
                    StatusCode = 400
                };

            if ((decimal)request.Pricing == 0 || request.Pricing == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price is a required field.",
                    StatusCode = 400
                };

            if ((int)request.DuarationId == 0 || request.DuarationId == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Campaign Price Duration is a required field.",
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
