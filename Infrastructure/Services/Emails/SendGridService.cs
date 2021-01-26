using Core.Interfaces.Emails;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace Infrastructure.Services.Emails
{
    public class SendGridService: ISendGridService
    {
        private readonly IConfiguration _config;
        public string FromEmail { get; set; }
        public string Password{ get; set; }
        public string Client { get; set; }
        public int Port { get; set; }
        public SendGridService(IConfiguration config)
        {
            _config = config;
            FromEmail = _config["Smtp:FromEmail"];
            Password = _config["Smtp:Password"];
            Client = _config["Smtp:Client"];
            Port = int.Parse(_config["Smtp:Port"]);
        }

        public void SendEmail(string email, string subject, string content)
        {
            try
            {

                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(FromEmail);
                    mail.To.Add(email);
                    mail.Subject = subject;
                    mail.Body = content;
                    mail.IsBodyHtml = true;
                    
                    using (SmtpClient smtp = new SmtpClient(Client, Port))
                    {
                        smtp.Credentials = new NetworkCredential(FromEmail, Password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
            }
            catch (System.Exception x)
            {
                throw  new System.Exception(x.Message);
            }
        }
    }
}
