using AutoMapper;
using Core.Dtos.Departments.Input;
using Core.Dtos.Departments.Output;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using scheduler_core.api.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class DepartmentsController : BaseApiController
    {
        private readonly IDepartmentService _departmentService;
        private readonly IMapper _mapper;
        public DepartmentsController(IMapper mapper, IDepartmentService departmentService)
        {
            _departmentService = departmentService;
            _mapper = mapper;
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDepartment(int id)
        {
            try
            {
                //Validate Input
                var department = await _departmentService.GetDepartmentById(id);
                if (department == null)
                    return BadRequest(new ApiResponse(400, "Department is not existing."));

                await _departmentService.DeleteDepartment(department);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public async Task<IActionResult> CreateDepartment([FromBody] CreateDepartmentInputDto request)
        {
            try
            {
                //Validate Input
                var validationResponse = await _departmentService.ValidateCreateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                var department = _mapper.Map<Department>(request);
                department.StatusId = 1;
                await _departmentService.CreateDepartment(department);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<Pagination<GetDepartmentOutputDto>>> GetAllDepartments([FromQuery] CommonSpecParams specParams)
        {
            try
            {

                var departments = await _departmentService.GetDepartments(specParams);
                var departmentsMapped = _mapper.Map<IReadOnlyList<GetDepartmentOutputDto>>(departments);
                var count = await _departmentService.GetDepartmentsCount(specParams);

                return Ok(new Pagination<GetDepartmentOutputDto>(specParams.PageIndex, specParams.PageSize, count, departmentsMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetDepartmentOutputDto>> GetDepartmentById(int id)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(id);
                if (department == null)
                    return BadRequest(new ApiResponse(400, "Department is not existing."));

                var departmentMapped = _mapper.Map<GetDepartmentOutputDto>(department);
                return Ok(departmentMapped);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
        [Authorize(Roles = "admin")]
        [HttpPut]
        public async Task<ActionResult<GetDepartmentOutputDto>> UpdateDepartment([FromBody] UpdateDepartmentInputDto request)
        {
            try
            {
                var department = await _departmentService.GetDepartmentById(request.Id);
                if (department == null)
                    return BadRequest(new ApiResponse(400, "Department is not existing."));

                var validationResponse = await _departmentService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                department.Name = request.Name;
                department.Description = request.Description;
                department.ShortName = request.ShortName;
                department.StatusId = request.StatusId;

                await _departmentService.UpdateDepartment(department);
                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
