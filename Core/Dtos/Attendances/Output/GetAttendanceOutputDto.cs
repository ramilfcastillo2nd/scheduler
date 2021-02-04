using System;

namespace Core.Dtos.Attendances.Output
{
    public class GetAttendanceOutputDto
    {
        public DateTime Date { get; set; }
        public bool IsActive { get; set; }
        public int IncentiveCount { get; set; }

    }
}
