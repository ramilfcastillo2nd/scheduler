namespace Core.Dtos.UserProfiles.Output
{
    public class GetUserProfileOutputDto
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ImageUrl { get; set; }
        public int? StatusId { get; set; } 
        public int? DepartmentId { get; set; }
        public int RoleId { get; set; }
    }
}
