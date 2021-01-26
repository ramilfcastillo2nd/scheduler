using Core.Entities;
using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface IUserRepository
    {
        Task<(int, IEnumerable<AppUser>)> GetAllUsers(int skip, int take, string search);
        Task<string> GetCurrentTimezoneById(Guid id);
        Task<AppUser> GetUserWithDetailsById(Guid id);
        AppUser GetUserWithDetail(Guid userId);
        void AddRefreshToken(AppUserToken refreshToken);
        AppUserToken GetLatestRefreshTokenByUserId(Guid userId);
        void UpdateRefreshToken(AppUserToken refreshToken);
    }
}
