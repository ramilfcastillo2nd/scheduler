using Core.Dtos.Contents.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IContentService
    {
        Task<ValidationOutputDto> ValidateSaveInput(SaveContentInputDto request);
        Task SaveContent(SaveContentInputDto request, string userId);
        Task<ValidationOutputDto> ValidateGetInput(int campaignId);
        Task<Content> GetContentByCampaignId(int campaignId);
    }
}
