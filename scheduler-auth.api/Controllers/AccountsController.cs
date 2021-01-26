using scheduler_auth.api.Errors;
using scheduler_auth.api.Extensions;
using Core.Dtos.Account.Input;
using Core.Dtos.Account.Output;
using Core.Entities.Identity;
using Core.Helpers;
using Core.Helpers.Accounts;
using Core.Helpers.Common;
using Core.Interfaces.Accounts;
using Core.Interfaces.Settings.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_auth.api.Helpers;
using Core.Interfaces;
using Core.Enums;

namespace scheduler_auth.api.Controllers
{
    public class AccountsController : BaseApiController
    {
        private readonly IEmailService _emailService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService; 
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly IUserProfileService _userProfileService;
        public AccountsController(IUserProfileService userProfileService, IEmailService emailService, ITokenService tokenService, IUserService userService, IWebHostEnvironment env, IAccountService accountService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<AppRole> roleManager)
        {
            _userProfileService = userProfileService;
            _emailService = emailService;
            _signInManager = signInManager;
            _userManager = userManager;
            _accountService = accountService;
            _roleManager = roleManager;
            _env = env;
            _roleManager = roleManager;
            _userService = userService;
            _tokenService = tokenService;
        }

        /// <summary>
        /// Get a renewed token by providing a refresh token.
        /// </summary>
        /// <param name="token">The expired token</param>
        /// <param name="refreshToken">The refresh token which comes with the token on authentication</param>   
        /// <param name="timeZone"></param>
        /// <param name="LastLocalTimeLoggedIn">Optional parameter, this is when the user refresh token need to know the datetime today.</param>
        [AllowAnonymous]
        [HttpPost("token/refresh")]
        public async Task<IActionResult> RefreshJWToken([FromBody] RefreshTokenInputDto request)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(request.timeZone))
                    request.timeZone = request.timeZone.Trim();

                var jwtData = await _accountService.RefreshJWTokenAsync(request.token, request.refreshToken, request.timeZone);
                var user = await _userManager.FindByIdAsync(jwtData.userId);

                if (user == null)
                    return BadRequest(new ApiResponse(400, "User of this token is not existing."));

                if (!string.IsNullOrEmpty(request.timeZone) && user != null)
                {
                    await _userService.UpdateTimezoneCurrentUser(user.Id, request.timeZone);
                }
                //Update User Last Login Date
                if (request.lastLocalTimeLoggedIn != null)
                {
                    if (request.lastLocalTimeLoggedIn.HasValue)
                        await _userService.UpdateUserLastLoginDate(user.Id, request.lastLocalTimeLoggedIn.Value);
                }

                if (!string.IsNullOrEmpty(jwtData.Item1))
                    return Ok(new { token = jwtData.NewToken, refreshToken = jwtData.NewRefreshToken, timeZone = jwtData.TimeZone });
                else
                    return BadRequest(new ApiResponse(400, "Invalid token or refresh token."));
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Invalid token or refresh token."));
            }
        }

        /// <summary>
        /// Verify the reset code given
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("resetpassword/verify")]
        public async Task<IActionResult> ResetVerifyCode([FromBody] ResetVerifyInputDto request)
        {
            try
            {
                //Check if request is not null
                var validateInputResponse = CommonHelper<ResetVerifyInputDto>.ValidateIfNullObject(request);
                if (!validateInputResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validateInputResponse.StatusCode, validateInputResponse.Message));

                //Check if verification code is valid
                //Check if number
                var validateCodeResponse = AccountHelper.ValidateCorrectVerificationCode(request.Code);
                if (!validateCodeResponse.IsSuccess)
                    return BadRequest(new ApiResponse(validateCodeResponse.StatusCode, validateCodeResponse.Message));

                //Get current user by email
                var user = await _userManager.FindByEmailAsync(request.Email);

                //Check if user is not null
                var validateUserExists = CommonHelper<AppUser>.ValidateIfNullObjectFromDatabase(user);
                if (!validateUserExists.IsSuccess)
                    return BadRequest(new ApiResponse(validateUserExists.StatusCode, validateUserExists.Message));

                bool isSuccess = await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", $"{request.Code}{user.LastTwoDigitForgotPasswordConfirmation}");
                if (isSuccess)

                    return Ok(new ResetVerifyOutputDto { 
                        UserId = user.Id.ToString(),
                        Message = "Code has been successfully verified."
                    });
                else
                    return BadRequest(new ResetVerifyOutputDto { 
                        UserId = null, 
                        Message = "You have provided an invalid code.  Please try again with the correct data." 
                    });
            }
            catch
            {
                return BadRequest(new ResetVerifyOutputDto
                {
                    UserId = null,
                    Message = "There is a problem verifiying the code. Please try again later or contact your administrator."
                });
            }
        }

        /// <summary>
        /// Get currrent user 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("currentuser")]
        public async Task<IActionResult> GetCurrentUserProfile()
        {
            try
            {
                //Get Current User
                var user = await _userManager.FindByEmailFromClaimsPrinciple(HttpContext.User);
                var userProfile = await _userProfileService.GetUserProfileByUserId(user.Id);
                var role = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
                var userResponse = new CurrentUserOutputDto {
                    FirstName = userProfile.FirstName,
                    id = userProfile.id,
                    LastName = userProfile.LastName,
                    Email = user.Email,
                    Image = userProfile.ImageUrl,
                    Role = role
                };

                return Ok(userResponse);
            }
            catch (Exception x)
            {
                return BadRequest(new ApiResponse(400, x.Message));
            }
        }

        /// <summary>
        /// Generates a new verification code for registration
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("register/resendcode")]
        public async Task<IActionResult> RegisterResendCode([FromBody] RegisterResendCodeInputDto request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.UserId);

                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {

                        var confirmationCode = await _accountService.CreateConfirmTokenAsync(user);
                        if (!string.IsNullOrEmpty(confirmationCode))
                        {
                            await SendConfirmationEmail(user.Email, confirmationCode);

                            return Ok(new ApiResponse(200, "Your registration code has been successfully resent. Please check your email for your 4 digits code."));
                        }
                        else
                        {
                            return BadRequest(new ApiResponse(400, "We are having trouble resending the code."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ApiResponse(400, "This email address is already verified by the authorized user. If you forgot your password, try \"Forgot Password\" feature on the Login page."));
                    }
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "We are having trouble resending the code."));
                }


            }
            catch (Exception x)
            {
                return BadRequest(new ApiResponse(400, x.Message));
            }
        }

        /// <summary>
        /// Generates a new verification code for forgot password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("forgotpassword/resendcode")]
        public async Task<IActionResult> ForgotPasswordResendCode([FromBody] ForgotPasswordResendCodeInputDto request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user != null)
                {
                    if (!user.EmailConfirmed)
                    {

                        var confirmationCode = await _accountService.ForgotPasswordAsync(user);
                        if (!string.IsNullOrEmpty(confirmationCode))
                        {
                            await SendConfirmationEmail(user.Email, confirmationCode);

                            return Ok(new ApiResponse(200, "Your forgot password code has been successfully resent. Please check your email for your 4 digits code."));
                        }
                        else
                        {
                            return BadRequest(new ApiResponse(400, "We are having trouble resending the code."));
                        }
                    }
                    else
                    {
                        return BadRequest(new ApiResponse(400, "This email address is already verified by the authorized user. If you forgot your password, try \"Forgot Password\" feature on the Login page."));
                    }
                }
                else
                {
                    return BadRequest(new ApiResponse(400, "We are having trouble resending the code."));
                }


            }
            catch (Exception x)
            {
                return BadRequest(new ApiResponse(400, x.Message));
            }
        }

        /// <summary>
        /// Replaces currrent password with a new one.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordInputDto request)
        {
            //Check if request is not null
            var validateInputResponse = CommonHelper<ResetPasswordInputDto>.ValidateIfNullObject(request);
            if (!validateInputResponse.IsSuccess)
                return BadRequest(new ApiResponse(validateInputResponse.StatusCode, validateInputResponse.Message));

            //Check if verification code is valid
            //Check if number
            var validateCodeResponse = AccountHelper.ValidateCorrectVerificationCode(request.Code);
            if (!validateCodeResponse.IsSuccess)
                return BadRequest(new ApiResponse(validateCodeResponse.StatusCode, validateCodeResponse.Message));

            //Get current user by email
            var user = await _userManager.FindByEmailAsync(request.Email);

            //Check if user is not null
            var validateUserExists = CommonHelper<AppUser>.ValidateIfNullObjectFromDatabase(user);
            if (!validateUserExists.IsSuccess)
                return BadRequest(new ApiResponse(validateUserExists.StatusCode, validateUserExists.Message));

            if (!user.EmailConfirmed)
            {
                return BadRequest(new ApiResponse(400, "It looks like your account has not been confirmed yet. Please check your email for the confirmation code in order to set your password. Thank you."));
            }

            var isVerified = _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, request.OldPassword);
            if (isVerified == PasswordVerificationResult.Failed)
            {
                return BadRequest(new ApiResponse(400, "There is a problem with resetting your password. Please try again or contact support if the problem persists."));
            }

            var result = await _accountService.ResetPasswordAsync(user, request.NewPassword, $"{request.Code}{user.LastTwoDigitForgotPasswordConfirmation}");
            if (result.IsSuccess)
                return Ok(new ApiResponse(200, "Your password has been successfully changed."));

            return BadRequest(new ApiResponse(400, "There is a problem with resetting your password. Please try again or contact support if the problem persists."));
        }

        /// <summary>
        /// Sends a password recovery information to user's email.
        /// </summary>
        [AllowAnonymous]
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            var isValidEmail = RegexUtilities.IsValidEmail(email);
            if (!isValidEmail)
                return BadRequest(new ApiResponse(400, "You have entered an invalid email address. Please try again or contact support."));

            var user = await _userManager.FindByEmailAsync(email);

            //Check if user is not null
            var validateUserExists = CommonHelper<AppUser>.ValidateIfNullObjectFromDatabase(user);
            if (!validateUserExists.IsSuccess)
                return BadRequest(new ApiResponse(validateUserExists.StatusCode, validateUserExists.Message));

            var resetCode = await _accountService.ForgotPasswordAsync(user);
            if (!string.IsNullOrEmpty(resetCode))
            {
                var year = DateTime.Now.Date.Year.ToString();
                var pathToTemplateFile = Path.Combine(_env.WebRootPath, "email-templates", "reset-email", "reset.html");
                var strTemplate = string.Empty;
                using (StreamReader SourceReader = System.IO.File.OpenText(pathToTemplateFile))
                {
                    strTemplate = SourceReader.ReadToEnd();
                }

                var htmlBody = new StringBuilder(strTemplate);
                htmlBody.Replace("{{verification_code}}", resetCode);
                var content = htmlBody.ToString();
                var subject = "Your Campaign Scheduler reset password request is here!";

                //_emailService.SendEmail(email, subject, content);

                return Ok(new { 
                    statusCode = 200,
                    userId = user.Id,
                    message = "We have sent you an email with reset password instructions. Please check your inbox now to confirm."
                });
            }

            return BadRequest(new ApiResponse(400, "User not found"));
        }
        /// <summary>
        /// Confirm newly registerd account
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut("confirm")]
        public async Task<IActionResult> ConfirmEmail(Guid userId, string verificationCode)
        {
            var existingUser = await _userService.GetUserById(userId);

            //Check If user is existing
            if (existingUser == null)
            {
                return BadRequest(new ApiResponse(400, "You have not registered to Campaign Scheduler yet. Please sign up and we will send you a confirmation email."));
            }
            //Check if user is already confirmed
            if (existingUser.EmailConfirmed)
            {
                return BadRequest(new ApiResponse(400, "You have already confirmed your email. Please try the forgot password feature on the login page."));
            }
            //Check if verification code is valid
            //Check if number
            var isNumber = verificationCode.All(char.IsNumber);
            if (!isNumber || verificationCode.Length != 4)
            {
                return BadRequest(new ApiResponse(400, "You have entered an invalid code. Please check your email for the correct code."));
            }

            var result = await _accountService.ConfirmEmailAsync(existingUser, verificationCode);
            if (result.Status)
                return Ok(new ApiResponse(200, "Welcome to Campaign Scheduler!")); 
            else
            {
                verificationCode = await _accountService.CreateConfirmTokenAsync(existingUser);
                await SendConfirmationEmail(existingUser.Email, verificationCode);
                return BadRequest(new ApiResponse(400, "Your code is expired and we have sent you a new code in your mailbox. If you cannot find our email please check your junk email."));
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginAccountOutputDto>> Login(LoginAccountInputDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.UserName);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);


            // Ensure the user is not already locked out.
            if (result.IsLockedOut)
                return BadRequest(new ApiResponse(400, "The specified user account has been suspended."));

            // Reject the token request if two-factor authentication has been enabled by the user.
            if (result.RequiresTwoFactor)
                return BadRequest(new ApiResponse(400, "Invalid login procedure."));

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));
            var tokenResponse = await _tokenService.CreateToken(user, loginDto.TimeZone);
            var timezone = await _accountService.GetUserTimezoneById(user.Id);
            var hasChangePassword = user.HasChangePassword.HasValue ? user.HasChangePassword.Value : false;

            var role = 1;
            var roleName = (await _userManager.GetRolesAsync(user)).FirstOrDefault();
            if (roleName == "admin")
                role = (int)UserRole.admin;
            else if (roleName == "manager")
                role = (int)UserRole.manager;
            else
                role = (int)UserRole.sdr;

            return new LoginAccountOutputDto
            {
                Id = user.Id,
                Username = user.Email,
                Token = tokenResponse.Token,
                RefreshToken = tokenResponse.RefreshToken,
                TimeZone = timezone,
                HasChangePassword = hasChangePassword,
                Role = role
            };
        }

        [HttpPost("setpassword")]
        public async Task<IActionResult> SetPassword([FromBody] SetPasswordInputDto request)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var isValid = await _userManager.CheckPasswordAsync(user, request.CurrentPassword);

            if (!isValid)
                return BadRequest(new ApiResponse(400, "The password entered doesn't match the current password."));

            var response = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);

            if (response.Succeeded)
            {
                user.HasChangePassword = true;
                await _userManager.UpdateAsync(user);
                return Ok(new ApiResponse(200, "You have successfully set your password!"));
            }
            else
                return BadRequest(new ApiResponse(400, "There is a problem changing the account's password."));
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterAccountInputDto request)
        {
            if (CheckEmailExistsAsync(request.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser
            {
                Email = request.Email,
                UserName = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                HasChangePassword = false,
                StatusId = (int)UserAccountStatus.ACTIVE
            };

            if (request.RoleId <=1 || request.RoleId > 4) return BadRequest(new ApiResponse(400, "RoleId is not valid."));

            var createUserResponse = await CreateUser(user, request.RoleId);

            if (!createUserResponse.IsSuccess) return BadRequest(new ApiResponse(400, createUserResponse.Message));

            return Ok(new { 
              statusCode = 200,
              userId = user.Id,
            });
        }

        private async Task<RegisterAccountOutputDto> CreateUser([FromBody] AppUser user, int roleId)
        {
            if (ModelState.IsValid)
            {
                user.RegisteredDate = DateTime.UtcNow;
                var temporaryPassword = Randomizer.RandomNumberOfLettersAll(32);
                
                var role = (UserRole)roleId;
                var roleValue = role.ToString().ToLower();
                var result = await _accountService.CreateUserAsync(user, new string[] { roleValue }, temporaryPassword);
                if (result.Item1 != null)
                {
                    await SendConfirmationEmail(user.Email, temporaryPassword);

                    return new RegisterAccountOutputDto
                    {
                        UserId =  result.Item1.Id,
                        Message = "Successfully created user",
                        IsSuccess = true
                    };
                }

                return new RegisterAccountOutputDto
                {
                    Message = result.Item2[0],
                    UserId = null,
                    IsSuccess = false
                };

            }

            return new RegisterAccountOutputDto
            {
                Message = "Please review your inputs.",
                UserId = null,
                IsSuccess = false
            };
        }
        private async Task SendConfirmationEmail(string email, string password)
        {
            var pathToTemplateFile = Path.Combine(_env.WebRootPath, "email-templates", "verification-email", "verification.html");
            var strTemplate = string.Empty;
            using (StreamReader SourceReader = System.IO.File.OpenText(pathToTemplateFile))
            {
                strTemplate = SourceReader.ReadToEnd();
            }

            var htmlBody = new StringBuilder(strTemplate);
            htmlBody.Replace("{{password}}", password);

            var subject = "Verify your email to start using Campaign Scheduler";
            var content = htmlBody.ToString();

            await _emailService.SendGridEmail(email, subject, content);
        }

        [AllowAnonymous]
        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
    }
}
  