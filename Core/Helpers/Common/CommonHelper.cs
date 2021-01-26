using Core.Dtos.Validation.Output;

namespace Core.Helpers.Common
{
    public static class CommonHelper<T>
    {
        public static ValidationOutputDto ValidateIfEmptyString(string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString) || string.IsNullOrEmpty(inputString))
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "Input data is invalid. Please try again with the correct data.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public static ValidationOutputDto ValidateIfNullObject(T request)
        {
            if (request == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "There is no data input. Please try again with the correct data.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }

        public static ValidationOutputDto ValidateIfNullObjectFromDatabase(T request)
        {
            if (request == null)
                return new ValidationOutputDto
                {
                    IsSuccess = false,
                    Message = "There is no record existing. Please try again with the correct data.",
                    StatusCode = 400
                };

            return new ValidationOutputDto
            {
                IsSuccess = true,
                Message = string.Empty,
                StatusCode = 200
            };
        }
    }
}
