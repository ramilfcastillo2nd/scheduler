using Core.Entities.Identity;
using Core.Interfaces.Accounts;
using Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_auth.api.Helpers
{
    public interface IDatabaseInitializer
    {
        Task SeedData();
    }
    public class DatabaseInitializer : IDatabaseInitializer
    {

        private readonly CampaignSchedulerContext _context;
        private readonly IAccountService _accountService;
        private IHostingEnvironment _env;

        public DatabaseInitializer(CampaignSchedulerContext context, IAccountService accountService, IHostingEnvironment env)
        {
            _accountService = accountService;
            _context = context;
            _env = env;
        }

        public async Task SeedData()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await InitNewRoles();
        }

        private async Task InitNewRoles()
        {
            const string adminRoleName = "admin";

            await EnsureRoleAsync(adminRoleName, "Admin", new string[] { });

            //Create Super Admin User
            var userExists = await _accountService.CheckUserExists("bryce@outreachlever.io");
            if (!userExists)
            {
                var password = "";
                if (_env.IsDevelopment())
                    password = "ypmd@LihvhLbYRBFcJ3A";
                else if (_env.IsStaging())
                    password = "L8BYu*9EuKzgqNiNgAfB";
                else
                    password = "hTfpwbs!D7p6eZbjNUk!";

                await CreateUserAsync("bryce@outreachlever.io", password, "Default", "Admin", "bryce@outreachlever.io", "", new string[] { adminRoleName });
            }

            //Create Super Admin User
            var user2Exists = await _accountService.CheckUserExists("philip@growthlever.io");
            if (!user2Exists)
            {
                var password = "";
                if (_env.IsDevelopment())
                    password = "ypmd@LihvhLbYRBFcJ3A";
                else if (_env.IsStaging())
                    password = "L8BYu*9EuKzgqNiNgAfB";
                else
                    password = "hTfpwbs!D7p6eZbjNUk!";

                await CreateUserAsync("philip@growthlever.io", password, "Default", "Admin", "philip@growthlever.io", "", new string[] { adminRoleName });
            }

            //Create Super Admin User
            var user3Exists = await _accountService.CheckUserExists("hazelle@growthlever.io");
            if (!user3Exists)
            {
                var password = "";
                if (_env.IsDevelopment())
                    password = "ypmd@LihvhLbYRBFcJ3A";
                else if (_env.IsStaging())
                    password = "L8BYu*9EuKzgqNiNgAfB";
                else
                    password = "hTfpwbs!D7p6eZbjNUk!";

                await CreateUserAsync("hazelle@growthlever.io", password, "Default", "Admin", "hazelle@growthlever.io", "", new string[] { adminRoleName });
            }

        }

        private async System.Threading.Tasks.Task EnsureRoleAsync(string roleName, string description, string[] claims)
        {
            if ((await _accountService.GetRoleByNameAsync(roleName)) == null)
            {
                AppRole applicationRole = new AppRole(roleName, description);

                var result = await _accountService.CreateRoleAsync(applicationRole, claims);
            }
        }

        private async Task<AppUser> CreateUserAsync(string userName, string password, string firstName, string lastName, string email, string phoneNumber, string[] roles)
        {

            var applicationUser = new AppUser
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                EmailConfirmed = true
            };

            var result = await _accountService.CreateUserAsync(applicationUser, roles, password);

            return result.Item1;
        }
    }
}
