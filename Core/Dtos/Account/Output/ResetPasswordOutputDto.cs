namespace Core.Dtos.Account.Output
{
    public class ResetPasswordOutputDto
    {
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }
    }
}
