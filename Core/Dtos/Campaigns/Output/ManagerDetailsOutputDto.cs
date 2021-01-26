namespace Core.Dtos.Campaigns.Output
{
    public class ManagerDetailsOutputDto
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int? StatusId { get; set; }
    }
}
