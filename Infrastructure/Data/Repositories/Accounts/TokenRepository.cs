using Core.Entities.Identity;
using Core.Interfaces.Accounts;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Data.Repositories.Accounts
{
    public class TokenRepository: ITokenRepository
    {
        private readonly CampaignSchedulerContext _context;
        public TokenRepository(CampaignSchedulerContext context)
        {
            _context = context;
        }
        public IQueryable<AppUserToken> GetRefreshTokens()
        {
            return _context.Users
                    .Include(s => s.RefreshTokens)
                    .SelectMany(f => f.RefreshTokens);
        }

        public async Task UpdateAsync(AppUserToken appUserToken)
        {
            _context.Attach(appUserToken).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
