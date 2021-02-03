using Core.Dtos.Validation.Output;
using Core.Dtos.Wages.Input;
using Core.Entities;

using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IWageService
    {
        Task CreateWage(Wage request);
        Task UpdateWage(Wage request);
        Task DeleteWage(Wage wage);
        Task<Wage> GetWageInfoByIdAsync(int id);
        Task<ValidationOutputDto> ValidateCreateInputAsync(AddWageInputDto request);
        //Task<ValidationOutputDto> ValidateUpdateInput(UpdateCampaignInputDto request);
        //Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignStatusInputDto request);
    }
}
