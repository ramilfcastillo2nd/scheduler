namespace Core.Dtos.Account.Input
{
    public class SetPasswordInputDto
    {
        public string UserId { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
