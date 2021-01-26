using Core.Dtos.UserProfiles.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Enums;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IUserProfileService
    {
        Task<int> GetRoleByUserProfileId(int userProfileId);
        Task CreateUserProfile(UserProfile userProfile);
        Task<UserProfile> GetUserProfileByUserId(Guid userId);
        Task UpdateUserProfile(UpdateUserProfileInputDto request);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateUserProfileInputDto request);
        Task<IReadOnlyList<UserProfile>> GetUserProfilesByDepartment(int departmentId, CommonSpecParams specParams);
        Task<int> GetUserProfilesCountByDepartment(int departmentId, CommonSpecParams specParams);
        Task<IReadOnlyList<UserProfile>> GetUserProfiles(CommonSpecParams specParams);
        Task<int> GetUserProfilesCount(CommonSpecParams specParams);
        Task<IReadOnlyList<UserProfile>> GetUserProfiles(Guid[] appUserIds, CommonSpecParams specParams);
        Task<int> GetUserProfilesCount(Guid[] appUserIds, CommonSpecParams specParams);
        Task<ValidationOutputDto> ValidateAssignDepartmentInput(AssignDepartmentInputDto request);
        Task AssignUserToDepartment(AssignDepartmentInputDto request);
        Task<UserProfile> GetUserProfileById(int id);
    }
}
