using Core.Dtos.Departments.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IDepartmentService
    {
        Task<IReadOnlyList<Department>> GetDepartments(CommonSpecParams specParams);
        Task<int> GetDepartmentsCount(CommonSpecParams specParams);
        Task CreateDepartment(Department department);
        Task<ValidationOutputDto> ValidateCreateInput(CreateDepartmentInputDto request);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateDepartmentInputDto request);
        Task UpdateDepartment(Department request);
        Task<Department> GetDepartmentById(int id);
        Task DeleteDepartment(Department department);
    }
}
