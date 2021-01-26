namespace Core.Dtos.Account.Input
{
    public class ResetPasswordInputDto
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string Code { get; set; }
    }
}
