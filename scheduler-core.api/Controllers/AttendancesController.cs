using Core.Dtos.Attendances.Input;
using Core.Dtos.Attendances.Output;
using Microsoft.AspNetCore.Mvc;
using scheduler_core.api.Errors;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class AttendancesController : BaseApiController
    {
        public AttendancesController()
        {
            
        }

        [HttpPost("process")]
        public async Task<IActionResult> CheckAttendance([FromBody] CheckAttendanceInputDto request)
        {
            try
            {
                return Ok("Successful!");
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong"));
            }
        }

        [HttpGet("employeeid={employeeId}")]
        public async Task<IActionResult> CheckAttendance(int employeeId)
        {
            try
            {
                var listAttendance = new List<GetAttendanceOutputDto> {
                    new GetAttendanceOutputDto {
                        Date = DateTime.Now,
                        IncentiveCount = 1,
                        IsActive = true
                    },
                    new GetAttendanceOutputDto {
                        Date = DateTime.Now.AddDays(1),
                        IncentiveCount = 1,
                        IsActive = false
                    },
                    new GetAttendanceOutputDto {
                        Date = DateTime.Now.AddDays(2),
                        IncentiveCount = 1,
                        IsActive = true
                    }
                };

                return Ok(listAttendance);
            }
            catch
            {
                return BadRequest(new ApiResponse(400, "Something went wrong"));
            }
        }
    }
}
