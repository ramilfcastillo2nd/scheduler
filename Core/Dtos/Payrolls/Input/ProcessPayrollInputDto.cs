using System;

namespace Core.Dtos.Payrolls.Input
{
    public class ProcessPayrollInputDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? EmployeeId { get; set; }
    }
}
