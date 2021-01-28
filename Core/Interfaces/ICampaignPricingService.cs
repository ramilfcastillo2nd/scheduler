using Core.Dtos.CampaignPricings.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICampaignPricingService
    {
        Task CreateCampaignPricing(CampaignPricing request);
        Task<IReadOnlyList<CampaignPricing>> GetCampaignPricings();
        Task<CampaignPricing> GetCampaignPricingById(int id);
        Task UpdateCampaignPrice(CampaignPricing request);
        Task DeleteCampaignPrice(CampaignPricing campaignPrice);
        Task<ValidationOutputDto> ValidateCreateInput(CreateCampaignPricingInputDto request);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateCampaignPricingInputDto request);
    }
}
