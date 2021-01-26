using System.Collections.Generic;

namespace Core.Entities
{
    public class Department:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ShortName { get; set; }
        public int? StatusId { get; set; }
        public ICollection<UserProfile> UserProfiles { get; set; }
    }
}
