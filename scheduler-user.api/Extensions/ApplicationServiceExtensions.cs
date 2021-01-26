using System.Linq;
using scheduler.api.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Core.Interfaces.Settings.Email;
using Infrastructure.Services.Settings;
using Core.Interfaces.Accounts;
using Infrastructure.Data.Repositories.Accounts;
using Infrastructure.Services.Accounts;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;

//this is for cleaning up the Startup file. we extend the IService Collection.
namespace scheduler_user.api.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<ISdrGroupingService, SdrGroupingService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IUserAccountService, UserAccountService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
            
            return services;
        }
    }
}