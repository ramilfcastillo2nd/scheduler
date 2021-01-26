using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Settings.Email
{
    public class EmailMessage
    {
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }

        public string ToAddress { get; set; }
        public string FromAddress { get; set; }
        public List<EmailAddress> ToAddresses { get; set; }
        public List<EmailAddress> FromAddresses { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public bool IsPlainText { get; set; } = false;
        public string AttachmentUrl { get; set; }
        public byte[] Attachment { get; set; }
    }
}
