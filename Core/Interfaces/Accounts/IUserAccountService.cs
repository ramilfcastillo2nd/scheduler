using Core.Dtos.Account.Output;
using Core.Entities.Identity;
using Core.Specifications;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Accounts
{
    public interface IUserAccountService
    {
        Task<IReadOnlyList<AppUser>> GetUserAccountsByRole(string roleName);
        Task<(int, IEnumerable<UserAccountOutputDto>)> GetUserAccounts(CommonSpecParams specParams);
    }
}
