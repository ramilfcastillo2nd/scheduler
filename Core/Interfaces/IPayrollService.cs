using Core.Dtos.Payrolls;
using Core.Dtos.Payrolls.Input;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IPayrollService
    {
        Task ProcessPayrollPeriod(ProcessPayrollInputDto request, Guid userId);
        Task<IReadOnlyList<Payroll>> GetPayrollByUserProfileId(Guid userId);
    }
}
