namespace scheduler_azure.api.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "It seems that you are not a registered user. Please register first before trying again or contact support if this problem persist.",
                404 => "Page was not found, please check any typo on your endpoint",
                500 => "Errors are the path to the dark side. Errors lead to anger.",
                _ => null
            };
        }
    }
}
