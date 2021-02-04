using Core.Dtos.Validation.Output;
using Core.Dtos.Wages.Input;
using Core.Entities;
using Core.Specifications;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IWageService
    {
        Task<IReadOnlyList<Wage>> GetWagesAsync(CommonSpecParams specParams);
        Task CreateWage(Wage request);
        Task UpdateWage(Wage request);
        Task DeleteWage(Wage wage);
        Task<Wage> GetWageInfoByIdAsync(int id);
        Task<ValidationOutputDto> ValidateCreateInputAsync(AddWageInputDto request);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateWageInputDto request);
        //Task<ValidationOutputDto> ValidateChangeStatusInput(ChangeCampaignStatusInputDto request);
    }
}
