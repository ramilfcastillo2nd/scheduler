using AutoMapper;
using Core.Dtos.UserProfiles.Input;
using Core.Dtos.UserProfiles.Output;
using Core.Interfaces;
using Core.Interfaces.Accounts;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using scheduler.api.Errors;
using scheduler_user.api.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_user.api.Controllers
{
    public class UserProfilesController : BaseApiController
    {
        private readonly IUserService _userService;
        private readonly IUserProfileService _userProfileService;
        private readonly IUserAccountService _userAccountService;
        private readonly IMapper _mapper;
        public UserProfilesController(IUserAccountService userAccountService, IMapper mapper, IUserProfileService userProfileService, IUserService userService)
        {
            _userService = userService;
            _userProfileService = userProfileService;
            _mapper = mapper;
            _userAccountService = userAccountService;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("assign/department")]
        public async Task<IActionResult> AssignDepartmentToUser([FromBody] AssignDepartmentInputDto request)
        {
            try
            {
                var validationResponse = await _userProfileService.ValidateAssignDepartmentInput(request);
                if(!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _userProfileService.AssignUserToDepartment(request);

                return Ok(new ApiResponse(200, "Success"));

            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserProfileById(int id)
        {
            try
            {
                var userProfile = await _userProfileService.GetUserProfileById(id);
                if (userProfile == null)
                    return BadRequest(new ApiResponse(400, "User profile is not existing."));

                var userProfileMapped = _mapper.Map<GetUserProfileDetailsOutputDto>(userProfile);

                return Ok(userProfileMapped);

            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("departmentId={departmentId}")]
        public async Task<IActionResult> GetUserProfilesByDepartment(int departmentId, [FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var userProfiles = await _userProfileService.GetUserProfilesByDepartment(departmentId, specParams);
                var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(userProfiles);
                var count = await _userProfileService.GetUserProfilesCountByDepartment(departmentId, specParams);
                return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAdminUserProfiles([FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var users = await _userAccountService.GetUserAccountsByRole("admin");
                if (users != null)
                {
                    var userIds = users.Select(s => s.Id).ToArray();
                    var userProfiles = await _userProfileService.GetUserProfiles(userIds, specParams);
                    var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(userProfiles);
                    userProfilesMapped = userProfilesMapped.Select(s =>
                    {
                        s.RoleId = _userProfileService.GetRoleByUserProfileId(s.id).Result;
                        return s;
                    }).ToList();
                    var count = await _userProfileService.GetUserProfilesCount(userIds, specParams);
                    return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
                }
                else
                {
                    return Ok(new ApiResponse(200, "No records found."));
                }      
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("sdr")]
        public async Task<IActionResult> GetSdrUserProfiles([FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var users = await _userAccountService.GetUserAccountsByRole("sdr");
                if (users != null)
                {
                    var userIds = users.Select(s => s.Id).ToArray();
                    var userProfiles = await _userProfileService.GetUserProfiles(userIds, specParams);
                    var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(userProfiles);
                     userProfilesMapped =  userProfilesMapped.Select(s =>
                    {
                        s.RoleId = _userProfileService.GetRoleByUserProfileId(s.id).Result;
                        return s;
                    }).ToList();
                    var count = await _userProfileService.GetUserProfilesCount(userIds, specParams);
                    return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
                }
                else
                {
                    return Ok(new ApiResponse(200, "No records found."));
                }
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet("manager")]
        public async Task<IActionResult> GetManagerUserProfiles([FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var users = await _userAccountService.GetUserAccountsByRole("manager");
                if (users != null)
                {
                    var userIds = users.Select(s => s.Id).ToArray();
                    var userProfiles = await _userProfileService.GetUserProfiles(userIds, specParams);
                    var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(userProfiles);
                    userProfilesMapped = userProfilesMapped.Select(s =>
                    {
                        s.RoleId = _userProfileService.GetRoleByUserProfileId(s.id).Result;
                        return s;
                    }).ToList();
                    var count = await _userProfileService.GetUserProfilesCount(userIds, specParams);
                    return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
                }
                else
                {
                    return Ok(new ApiResponse(200, "No records found."));
                }
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetUserProfiles([FromQuery] CommonSpecParams specParams)
        {
            try
            {
                var userProfiles = await _userProfileService.GetUserProfiles(specParams);
                var userProfilesMapped = _mapper.Map<IReadOnlyList<GetUserProfileOutputDto>>(userProfiles);
                userProfilesMapped = userProfilesMapped.Select(s =>
                {
                    s.RoleId = _userProfileService.GetRoleByUserProfileId(s.id).Result;
                    return s;
                }).ToList();
                var count = await _userProfileService.GetUserProfilesCount(specParams);
                return Ok(new Pagination<GetUserProfileOutputDto>(specParams.PageIndex, specParams.PageSize, count, userProfilesMapped));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserProfileInputDto request)
        {
            try
            {
                //Validate Input
                var validationResponse = await _userProfileService.ValidateUpdateInput(request);
                if (!validationResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validationResponse.StatusCode, validationResponse.Message));

                await _userProfileService.UpdateUserProfile(request);

                return Ok(new ApiResponse(200, "Success"));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }
    }
}
