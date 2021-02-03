using Core.Dtos.UserProfiles.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class UserProfileService: IUserProfileService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public UserProfileService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<UserProfile> GetUserProfileById(int id)
        {
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(id);
            return userProfile;
        }

        public async Task AssignUserToDepartment(AssignDepartmentInputDto request)
        {
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.UserProfileId);
            userProfile.DepartmentId = request.DepartmentId;

            _unitOfWork.Repository<UserProfile>().Update(userProfile);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateAssignDepartmentInput(AssignDepartmentInputDto request)
        {
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.UserProfileId);
            if(userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "User profile is not existing.",
                    StatusCode = 400
                };

            var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId);
            if (department == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Department is not existing.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task<ValidationOutputDto> ValidateUpdateInput(UpdateUserProfileInputDto request)
        {
            //Check if id is existing
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.Id);

            if (userProfile == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "User is not existing.",
                    StatusCode = 400
                };


            //Check if Sdr type is valid
            var sdrTypeExists = Enum.IsDefined(typeof(SdrType), request.SdrTypeId);
            if (!sdrTypeExists)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "SdrType is not valid.",
                    StatusCode = 400
                };

            if (request.DepartmentId.HasValue)
            {
                //Is Valid DepartmentId
                var department = await _unitOfWork.Repository<Department>().GetByIdAsync(request.DepartmentId.Value);
                if (department == null)
                    return new ValidationOutputDto
                    {
                        IsSuccess = false,
                        Message = "DepartmentId is not valid.",
                        StatusCode = 400
                    };
            }

            return new ValidationOutputDto { 
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task UpdateUserProfile(UpdateUserProfileInputDto request)
        {
            //Check if id is existing
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(request.Id);

            userProfile.FirstName = request.FirstName;
            userProfile.LastName = request.LastName;
            userProfile.ImageUrl = request.ImageUrl;
            userProfile.LastName = request.LastName;
            userProfile.LinkedInName = request.LinkedInName;
            userProfile.LinkedInUrl = request.LinkedInUrl;
            userProfile.PostalCode = request.PostalCode;
            userProfile.State = request.State;
            userProfile.DepartmentId = request.DepartmentId;
            userProfile.DateHired = request.DateHired;
            userProfile.Country = request.Country;
            userProfile.Address = request.Address;

            _unitOfWork.Repository<UserProfile>().Update(userProfile);
            await _unitOfWork.Complete();
        }

        public async Task CreateUserProfile(UserProfile userProfile)
        {
            _unitOfWork.Repository<UserProfile>().Add(userProfile);
            await _unitOfWork.Complete();
        }

        public async Task<UserProfile> GetUserProfileByUserId(Guid userId)
        {
            var specs = new GetUserProfileByUserIdSpecification(userId);
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(specs);
            return userProfile;
        }

        public async Task<IReadOnlyList<UserProfile>> GetUserProfilesByDepartment(int departmentId, CommonSpecParams specParams)
        {
            var spec = new GetUserProfilesByDepartmentSpecification(departmentId, specParams);
            var userProfiles = await _unitOfWork.Repository<UserProfile>().ListAsync(spec);
            return userProfiles;
        }

        public async Task<IReadOnlyList<UserProfile>> GetUserProfiles(CommonSpecParams specParams)
        {
            var spec = new GetUserProfilesSpecification(specParams);
            var userProfiles = await _unitOfWork.Repository<UserProfile>().ListAsync(spec);
            return userProfiles;
        }

        public async Task<IReadOnlyList<UserProfile>> GetUserProfiles(Guid[] appUserIds, CommonSpecParams specParams)
        {
            var spec = new GetUserProfilesByUserIdsSpecification(appUserIds, specParams);
            var userProfiles = await _unitOfWork.Repository<UserProfile>().ListAsync(spec);
            return userProfiles;
        }

        public async Task<int> GetUserProfilesCountByDepartment(int departmentId, CommonSpecParams specParams)
        {
            var specs = new GetUserProfilesByDepartmentCountSpecification(departmentId, specParams);
            var count = await _unitOfWork.Repository<UserProfile>().CountAsync(specs);
            return count;
        }

        public async Task<int> GetUserProfilesCount(CommonSpecParams specParams)
        {
            var specs = new GetUserProfilesCountSpecification(specParams);
            var count = await _unitOfWork.Repository<UserProfile>().CountAsync(specs);
            return count;
        }

        public async Task<int> GetUserProfilesCount(Guid[] appUserIds, CommonSpecParams specParams)
        {
            var specs = new GetUserProfilesByUserIdsCountSpecification(appUserIds, specParams);
            var count = await _unitOfWork.Repository<UserProfile>().CountAsync(specs);
            return count;
        }

        public async Task<int> GetRoleByUserProfileId(int userProfileId)
        {
            var specs = new GetUserProfileWithDetailsByIdSpecification(userProfileId);
            var userProfile = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(specs);

            var role = await _userManager.GetRolesAsync(userProfile.AppUser);
            var roleValue = role.First().ToLower();
            return (int)Enum.Parse<UserRole>(roleValue);
        }
    }
}
