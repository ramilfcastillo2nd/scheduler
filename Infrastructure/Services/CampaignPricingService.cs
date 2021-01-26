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
    }
}
