using System;

namespace Core.Dtos.Schedulers.Input
{
    public class UpdateSchedulerInputDto
    {
        public int id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string Industry { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string LinkedInUrl { get; set; }
        public bool? ConnectionRequest { get; set; }
        public DateTime? Date { get; set; }
        public bool? RequestAccepted { get; set; }
        public bool? IsMessage1 { get; set; }
        public DateTime? DateMessage1 { get; set; }
        public bool? IsMessage2 { get; set; }
        public DateTime? DateMessage2 { get; set; }
        public bool? IsMessage3 { get; set; }
        public DateTime? DateMessage3 { get; set; }
        public bool? IsMessage4 { get; set; }
        public DateTime? DateMessage4 { get; set; }
        public bool? IsMessage5 { get; set; }
        public DateTime? DateMessage5 { get; set; }
        public string Neutral { get; set; }
        public string Negative { get; set; }
    }
}
