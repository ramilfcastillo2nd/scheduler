using Core.Dtos.Schedulers.Input;
using Core.Dtos.Validation.Output;
using Core.Entities;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface ISchedulerService
    {
        Task<ValidationOutputDto> ValidateGetInput(int campaignId, int userProfileId);
        Task<ValidationOutputDto> ValidateUploadInput(UploadSchedulerInputDto request);
        Task InsertScheduler(List<Scheduler> schedulers, UploadSchedulerInputDto request, string userId);
        Task<(int, IReadOnlyList<Scheduler>)> GetSchedulers(int campaignId, int userProfileId, CommonSpecParams specParams);
        Task<Scheduler> GetSchedulerById(int id);
        Task UpdateScheduler(Scheduler scheduler);
        Task<ValidationOutputDto> ValidateUpdateInput(UpdateSchedulerInputDto request);
        Task<(int, IReadOnlyList<Scheduler>, Campaign, Content)> GetSchedulers(Guid userId, CommonSpecParams specParams);
    }
}
