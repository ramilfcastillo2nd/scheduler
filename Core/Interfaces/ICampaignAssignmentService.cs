using Core.Dtos.Campaigns.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ICampaignAssignmentService
    {
        Task CreateCampaignAssignment(CampaignAssignment request);
        Task<(IReadOnlyList<UserProfile>, int)> GetSdrsFromCampaign(int campaignId, CommonSpecParams specParams);
        Task<CampaignAssignment> GetExistingCampaignAssignment(int campaignId, int userProfileId);
        Task<CampaignAssignment> GetActiveCampaignAssignment(int userProfileId);
        Task DeleteAssignment(CampaignAssignment campaignAssignment);
        Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignSdrStatusInputDto request);
        Task ChangeStatus(ChangeCampaignSdrStatusInputDto request);
    }
}
