using Core.Entities.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface ITokenRepository
    {
        IQueryable<AppUserToken> GetRefreshTokens();
        Task UpdateAsync(AppUserToken appUserToken);
    }
}
