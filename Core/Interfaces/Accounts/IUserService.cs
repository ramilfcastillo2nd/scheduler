using Core.Dtos.Account.Input;
using Core.Entities;
using Core.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface IUserService
    {
        Task UpdateUser(UserUpdateInputDto request);
        Task<AppUser> GetUserById(Guid id);
        Task UpdateTimezoneCurrentUser(Guid userId, string timezone);
        Task UpdateUserLastLoginDate(Guid userId, DateTime lastLocalTimeLoggedIn);
    }
}
