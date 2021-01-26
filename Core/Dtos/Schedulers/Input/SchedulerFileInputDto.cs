using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos.Schedulers.Input
{
    public class SchedulerFileInputDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string JobTitle { get; set; }
        public string CompanyName { get; set; }
        public string Location { get; set; }
        public string LinkedInUrl { get; set; }
    }
}
