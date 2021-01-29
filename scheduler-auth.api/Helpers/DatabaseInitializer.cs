using Core.Entities.Identity;
using Core.Interfaces.Accounts;
using Infrastructure.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
        private IConfiguration _config;

        public DatabaseInitializer(CampaignSchedulerContext context, IAccountService accountService, IHostingEnvironment env, IConfiguration config)
        {
            _accountService = accountService;
            _context = context;
            _env = env;
            _config = config;
        }

        public async Task SeedData()
        {
            await _context.Database.MigrateAsync().ConfigureAwait(false);
            await InitNewRoles();
        }

        private async Task InitNewRoles()
        {
            string superAdmin = _config.GetSection("Roles:0").Value;
            string admin = _config.GetSection("Roles:1").Value;
            string sdr = _config.GetSection("Roles:2").Value;
            string sdrm = _config.GetSection("Roles:3").Value;

            await EnsureRoleAsync(superAdmin, "Super Admin", new string[] { });
            await EnsureRoleAsync(admin, "Admin", new string[] { });
            await EnsureRoleAsync(sdr, "Sdr", new string[] { });
            await EnsureRoleAsync(sdrm, "Sdr Manager", new string[] { });

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

                await CreateUserAsync("bryce@outreachlever.io", password, "Default", "Admin", "bryce@outreachlever.io", "", new string[] { superAdmin });
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

                await CreateUserAsync("philip@growthlever.io", password, "Default", "Admin", "philip@growthlever.io", "", new string[] { superAdmin });
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

                await CreateUserAsync("hazelle@growthlever.io", password, "Default", "Admin", "hazelle@growthlever.io", "", new string[] { superAdmin });
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
