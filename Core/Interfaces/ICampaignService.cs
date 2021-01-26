using Core.Dtos.Campaigns.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICampaignService
    {
        Task<int> GetCampaignsCount(CommonSpecParams specParams);
        Task<IReadOnlyList<Campaign>> GetCampaigns(CommonSpecParams specParams);
        Task<IReadOnlyList<Campaign>> GetCampaignsByUser(int userProfileId);
        Task<IReadOnlyList<Campaign>> GetCampaignsByManager(int userProfileId);
        Task<Campaign> GetCampaignById(int id);
        Task CreateCampaign(Campaign request);
        Task UpdateCampaign(Campaign request);
        Task DeleteCampaign(Campaign campaign);
        Task ChangeStatus(ChangeCampaignStatusInputDto request);
        Task<ValidationOutputDto> ValidateCreateInput(CreateCampaignInputDto request);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateCampaignInputDto request);
        Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignStatusInputDto request);
        Task<ValidationOutputDto> ValidateGetCampaignsByUserInput(int userProfileId);
        Task<ValidationOutputDto> ValidateGetCampaignsByManagerInput(int userProfileId);
    }
}
