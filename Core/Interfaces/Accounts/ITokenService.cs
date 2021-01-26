using Core.Entities.Identity;
using System;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface ITokenService
    {
        Task<(string Token, string RefreshToken)> CreateToken(AppUser user, string timeZone = "", string location = "");
        AppUserToken GetRefreshToken(Guid userId, string refreshToken);
        AppUserToken GetRefreshToken(Guid userid);
        Task UpdateRefreshToken(Guid userId, int refreshTokenId);
    }
}
