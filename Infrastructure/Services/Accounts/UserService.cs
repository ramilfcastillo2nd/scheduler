using Core.Dtos.Account.Input;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Interfaces.Accounts;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services.Accounts
{
    public class UserService: IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public UserService(
            IUserRepository repository, 
            UserManager<AppUser> userManager,
            IUnitOfWork unitOfWork
            )
        {
            _userManager = userManager;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<AppUser> GetUserById(Guid id)
        {
            return await _userManager.FindByIdAsync(id.ToString());
        }
        public async Task UpdateTimezoneCurrentUser(Guid userId, string timezone)
        {
            try
            {
                //var user = _repository.GetUserByAppId(userId);
                //if (user == null)
                //{
                //    throw new Exception("User does not exist!");
                //}

                //user.timezone = timezone;
                //_repository.UpdateUser(user);
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }
        public async Task UpdateUser(UserUpdateInputDto request)
        {
            var userVerified = _repository.GetUserWithDetail(request.Id);

            if (userVerified != null)
            {
                //userVerified.UserProfile.BirthDate = request.BirthDate;
                //userVerified.UserProfile.FirstName = request.FirstName;
                //userVerified.UserProfile.LastName = request.LastName;
                //userVerified.UserProfile.MobilePhone = request.MobilePhone;
                await _userManager.UpdateAsync(userVerified);
            }
            else
            {
                throw new Exception("User is not existing in our records.");
            }
        }
        public async Task UpdateUserLastLoginDate(Guid userId, DateTime lastLocalTimeLoggedIn)
        {
            try
            {
                //var user = _repository.GetUserByAppId(userId);
                //if (user != null)
                //{
                //    user.LastLocalTimeLoggedIn = lastLocalTimeLoggedIn;
                //    _repository.UpdateUser(user);
                //    await _unitOfWork.Complete();
                //}
            }
            catch (Exception x)
            {
                throw new Exception(x.Message);
            }
        }
    }
}
