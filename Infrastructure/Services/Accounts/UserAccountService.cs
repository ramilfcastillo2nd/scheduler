using Core.Dtos.Account.Output;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces.Accounts;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.Accounts
{
    public class UserAccountService: IUserAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;

        public UserAccountService(IUserRepository userRepository, UserManager<AppUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }


        public async Task<IReadOnlyList<AppUser>> GetUserAccountsByRole(string roleName)
        {
            var users = await _userManager.GetUsersInRoleAsync(roleName);
            return users.ToList();
        }

        public async Task<(int,IEnumerable<UserAccountOutputDto>)> GetUserAccounts(CommonSpecParams specParams)
        {
            var skip = specParams.PageSize * (specParams.PageIndex - 1);
            var take = specParams.PageSize;
            var search = specParams.Search;


            var users = await _userRepository.GetAllUsers(skip, take, search);
            var usersViewModel = users.Item2.Select(s => new UserAccountOutputDto
            {
                Email = s.Email,
                FirstName = s.FirstName,
                Id = s.Id,
                LastName = s.LastName,
                StatusId = s.StatusId.HasValue ? s.StatusId.Value: 1
            }).ToList();

            return (users.Item1, usersViewModel);
        }
    }
}
