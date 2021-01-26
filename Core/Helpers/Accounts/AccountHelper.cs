using Core.Dtos.Validation.Output;
using System.Linq;

namespace Core.Helpers.Accounts
{
    public static class AccountHelper
    {
        public static ValidationOutputDto ValidateCorrectVerificationCode(string code)
        {
            var isNumber = code.All(char.IsNumber);
            if (!isNumber || code.Length != 4)
            {
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "You have entered an invalid code.Please check your email for the correct code.",
                    StatusCode = 400
                };
            }

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };

        }

    }
}
