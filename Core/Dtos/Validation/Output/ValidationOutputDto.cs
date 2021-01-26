namespace Core.Dtos.Validation.Output
{
    public class ValidationOutputDto
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
    }
}
