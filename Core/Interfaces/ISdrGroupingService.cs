using Core.Dtos.SdrGroupings.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISdrGroupingService
    {
        Task<ValidationOutputDto> ValidateAssignSdrToManagerInput(AssignSdrToManagerInputDto request);
        Task<ValidationOutputDto> ValidateManagerIdGetInput(int managerId);
        Task AssignSdrToManager(AssignSdrToManagerInputDto request, string userId);
        Task<(UserProfile, IReadOnlyList<SdrGrouping>, int)> GetSdrGroupingByManageId(int managerId, CommonSpecParams specParams);
    }
}
