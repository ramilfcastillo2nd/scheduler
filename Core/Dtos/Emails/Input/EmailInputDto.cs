using System;
namespace Core.Dtos.Settings.Email
{
    public class EmailInputDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
