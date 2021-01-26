using Core.Dtos.SdrGroupings.Input;
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
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class SdrGroupingService: ISdrGroupingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        public SdrGroupingService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ValidationOutputDto> ValidateAssignSdrToManagerInput(AssignSdrToManagerInputDto request)
        {
            //Check if Manager UserprofileId exists
            var userManagerSpecs = new GetUserProfileWithDetailsByIdSpecification(request.ManagerEmployeeId);
            var userprofileManager = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(userManagerSpecs);
            if (userprofileManager == null)
                return new ValidationOutputDto {
                    IsSuccess = false,
                    Message = "Manager is not existing.",
                    StatusCode = 400
                };

            //Check if Role is Manager
            var roleManager = (await _userManager.GetRolesAsync(userprofileManager.AppUser)).First();
            if(roleManager != UserRole.manager.ToString())
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "The Manager Id provided is not assigned as role Manager.",
                    StatusCode = 400
                };

            //Check if Sdr UserprofileId exists
            var userSdrSpecs = new GetUserProfileWithDetailsByIdSpecification(request.SdrEmployeeId);
            var userprofileSdr = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(userSdrSpecs);
            if (userprofileSdr == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Sdr is not existing.",
                    StatusCode = 400
                };

            //Check if Role is SDR
            var roleSdr = (await _userManager.GetRolesAsync(userprofileSdr.AppUser)).First();
            if (roleSdr != UserRole.sdr.ToString())
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "The Sdr Id provided is not assigned as role SDR.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task AssignSdrToManager(AssignSdrToManagerInputDto request, string userId)
        {
            var sdrGroupingRequest = new SdrGrouping {
                CreatedBy = userId,
                CreatedDate = DateTime.UtcNow,
                ManagerId = request. ManagerEmployeeId,
                SdrId = request.SdrEmployeeId
            };

             _unitOfWork.Repository<SdrGrouping>().Add(sdrGroupingRequest);
            await _unitOfWork.Complete();
        }

        public async Task<ValidationOutputDto> ValidateManagerIdGetInput(int managerId)
        {
            //Check if Manager UserprofileId exists
            var userManagerSpecs = new GetUserProfileWithDetailsByIdSpecification(managerId);
            var userprofileManager = await _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(userManagerSpecs);
            if (userprofileManager == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Manager is not existing.",
                    StatusCode = 400
                };

            //Check if Role is Manager
            var roleManager = (await _userManager.GetRolesAsync(userprofileManager.AppUser)).First();
            if (roleManager != UserRole.manager.ToString())
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "The Manager Id provided is not assigned as role Manager.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public async Task<(UserProfile, IReadOnlyList<SdrGrouping>, int)> GetSdrGroupingByManageId(int managerId, CommonSpecParams specParams)
        {
            var manager = await _unitOfWork.Repository<UserProfile>().GetByIdAsync(managerId);
            var sdrsSpecs = new GetSdrGroupByManagerIdSpecification(managerId, specParams);
            var sdrList = await _unitOfWork.Repository<SdrGrouping>().ListAsync(sdrsSpecs);
            var sdrsCountSpecs = new GetSdrGroupByManagerIdSpecification(managerId, specParams);
            var count = await _unitOfWork.Repository<SdrGrouping>().CountAsync(sdrsSpecs);

            return (manager, sdrList, count);
        }
    }
}
