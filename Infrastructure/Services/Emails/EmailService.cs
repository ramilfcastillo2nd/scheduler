using Core.Interfaces.Settings.Email;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace Infrastructure.Services.Settings
{
    public class EmailService: IEmailService
    {
        private string FromEmail = string.Empty;
        private string CampaignSchedulerKey = string.Empty;
        private int Port = 8080;
        private string Password = string.Empty;
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
            FromEmail = _config["SendGrid:FromEmail"];
            CampaignSchedulerKey = _config["SendGrid:CampaignSchedulerKey"];
            Port = int.Parse(_config["Smtp:Port"]);
            Password = _config["Smtp:Password"];
        }

        //public void SendEmail(string email, string subject, string content)
        //{
        //    try
        //    {

        //        using (MailMessage mail = new MailMessage())
        //        {
        //            mail.From = new MailAddress(FromEmail);
        //            mail.To.Add(email);
        //            mail.Subject = subject;
        //            mail.Body = content;
        //            mail.IsBodyHtml = true;

        //            using (SmtpClient smtp = new SmtpClient(Client, Port))
        //            {
        //                smtp.Credentials = new NetworkCredential(FromEmail, Password);
        //                smtp.EnableSsl = true;
        //                smtp.Send(mail);
        //            }
        //        }
        //    }
        //    catch (System.Exception x)
        //    {
        //        throw new System.Exception(x.Message);
        //    }
        //}

        public async Task SendMailGunEmail(string email, string subject, string content, string fullName)
        {
            const string url = @"https://api.mailgun.net/v3";
            RestClient client = new RestClient();
            client.BaseUrl = new System.Uri(url);
            client.Authenticator =
            new HttpBasicAuthenticator("api",
                                       "7fc3c6a6cddae13f47367b57b1556711-e5da0167-73befa81");
            RestRequest request = new RestRequest();
            request.AddParameter("domain", "sandbox57475d27214744e4b926334f848730b7.mailgun.org", ParameterType.UrlSegment);
            request.Resource = "{domain}/messages";
            request.AddParameter("from", "Mailgun Sandbox <postmaster@sandbox57475d27214744e4b926334f848730b7.mailgun.org>");
            request.AddParameter("to", $"{fullName} <{email}>");
            request.AddParameter("subject", subject);
            request.AddParameter("html", content);
            request.Method = Method.POST;
            client.Execute(request);
        }

        public async Task SendGridEmail(string email, string subject, string content)
        {
            try
            {
                var apiKey = CampaignSchedulerKey;
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(FromEmail, "Growth Lever Support Team");
                var to = new EmailAddress(email, "New User");
                var htmlContent = content;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, string.Empty, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch (System.Exception x)
            {
                throw new System.Exception(x.Message);
            }
        }
    }
}
