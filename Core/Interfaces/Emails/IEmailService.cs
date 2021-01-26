using Core.Dtos.Settings.Email;
using System.Threading.Tasks;

namespace Core.Interfaces.Settings.Email
{
    public interface IEmailService
    {
        Task SendGridEmail(string email, string subject, string content);
        Task SendMailGunEmail(string email, string subject, string content, string fullName);
    }
}
