namespace Core.Dtos.Departments.Output
{
    public class GetDepartmentOutputDto
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int? StatusId { get; set; }
    }
}
