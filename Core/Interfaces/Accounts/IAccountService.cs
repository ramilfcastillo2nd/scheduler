using Core.Dtos.Account.Output;
using Core.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface IAccountService
    {
        Task<(bool, string[])> CreateRoleAsync(AppRole role, IEnumerable<string> claims);
        Task<bool> CheckUserExists(string email);
        Task<AppRole> GetRoleByNameAsync(string roleName);
        Task<ResetPasswordOutputDto> ResetPasswordAsync(AppUser user, string newPassword, string code);
        Task<string> ForgotPasswordAsync(AppUser user);
        Task<string> CreateConfirmTokenAsync(AppUser user);
        Task<ConfirmCodeOutputDto> ConfirmEmailAsync(AppUser user, string code);
        Task<string> GetUserTimezoneById(Guid userId);
        Task<(AppUser, string[])> CreateUserAsync(AppUser user, IEnumerable<string> roles, string password, bool requireConfirmation = true);
        Task<(string NewToken, string NewRefreshToken, string TimeZone, string userId)> RefreshJWTokenAsync(string token, string refreshToken, string timezone = "", string location = "");
    }
}
