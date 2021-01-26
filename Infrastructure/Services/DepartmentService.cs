using Core.Dtos.Departments.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateDepartment(Department department)
        {
            _unitOfWork.Repository<Department>().Add(department);
            await _unitOfWork.Complete();
        }

        public async Task DeleteDepartment(Department department)
        {
            _unitOfWork.Repository<Department>().Delete(department);
            await _unitOfWork.Complete();
        }

        public async Task<Department> GetDepartmentById(int id)
        {
            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(id);
            return department;
        }

        public async Task<IReadOnlyList<Department>> GetDepartments(CommonSpecParams specParams)
        {
            var specs = new GetDepartmentsSpecification(specParams);
            var departments = await _unitOfWork.Repository<Department>().ListAsync(specs);
            return departments;
        }

        public async Task<int> GetDepartmentsCount(CommonSpecParams specParams)
        {
            var specs = new GetDepartmentsCountSpecification(specParams);
            var count = await _unitOfWork.Repository<Department>().CountAsync(specs);
            return count;
        }

        public async Task UpdateDepartment(Department request)
        {
            _unitOfWork.Repository<Department>().Update(request);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateCreateInput(CreateDepartmentInputDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ValidationOutputDto { 
                    IsSuccess = false,
                    Message = "Name is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Description))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Description is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.ShortName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Short Name is a required field.",
                    StatusCode = 400
                };

            return new ValidationOutputDto {
                StatusCode = 200,
                Message = string.Empty,
                IsSuccess = true
            };
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateDepartmentInputDto request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Name is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.Description))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Description is a required field.",
                    StatusCode = 400
                };

            if (string.IsNullOrEmpty(request.ShortName))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Short Name is a required field.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                StatusCode = 200,
                Message = string.Empty,
                IsSuccess = true
            };
        }
    }
}
