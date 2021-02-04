using System;

namespace Core.Dtos.Attendances.Input
{
    public class CheckAttendanceInputDto
    {
        public int UserProfileId { get; set; }
        public int CampaignId { get; set; }
        public DateTime Date { get; set; }
    }
}
