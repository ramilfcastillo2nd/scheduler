using Core.Dtos.Account.Input;
using Core.Dtos.Account.Output;
using Core.Dtos.UserProfiles;
using Core.Entities.Identity;
using Core.Enums;
using Core.Interfaces.Accounts;
using Core.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using scheduler.api.Errors;
using scheduler_user.api.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_user.api.Controllers
{
    public class UserAccountsController : BaseApiController
    {
        private readonly IUserAccountService _userAccountService;
        private readonly UserManager<AppUser> _userManager;
        public UserAccountsController(IUserAccountService userAccountService, UserManager<AppUser> userManager)
        {
            _userAccountService = userAccountService;
            _userManager = userManager;
        }

        [Authorize(Roles = "admin")]
        [HttpPut("status/update")]
        public async Task<IActionResult> ChangeUserAccountStatus([FromBody] ChangeStatusInputDto request)
        {
            try
            {
                //Check if user is existing
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user == null)
                    return BadRequest(new ApiResponse(400, "User is not existing."));

                //Check if status is valid
                var exists = Enum.IsDefined(typeof(UserAccountStatus), request.StatusId);
                if (!exists)
                    return BadRequest(new ApiResponse(400, "StatusId is not valid."));

                user.StatusId = request.StatusId;
                await _userManager.UpdateAsync(user);

                return Ok(new ApiResponse(200, "Successfully changed status."));
            }
            catch (Exception x)
            {
                return BadRequest(new ApiResponse(400, "Something went wrong."));
            }
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUserAccounts([FromQuery] CommonSpecParams specParams)
        {
            var userAccounts = await _userAccountService.GetUserAccounts(specParams);
            var count = userAccounts.Item1;

            return Ok(new Pagination<UserAccountOutputDto>(specParams.PageIndex, specParams.PageSize, count, userAccounts.Item2.ToList()));
        }

    }
}
