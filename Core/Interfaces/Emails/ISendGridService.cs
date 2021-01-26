namespace Core.Interfaces.Emails
{
    public interface ISendGridService
    {
        void SendEmail(string email, string subject, string content);
    }
}
