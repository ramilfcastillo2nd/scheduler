namespace Core.Dtos.Departments.Input
{
    public class UpdateDepartmentInputDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int? StatusId { get; set; }
    }
}
