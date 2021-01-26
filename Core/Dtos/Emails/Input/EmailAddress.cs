namespace Core.Dtos.Settings.Email
{
    public class EmailAddress
    {
        public EmailAddress() { }
        public EmailAddress(string emailAddress) { Address = emailAddress; }
        public EmailAddress(string name, string emailAddress) { Name = name; Address = emailAddress; }
        public string Name { get; set; }
        public string Address { get; set; }
    }
}
