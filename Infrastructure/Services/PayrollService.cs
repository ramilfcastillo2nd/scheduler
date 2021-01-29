using Core.Dtos.Payrolls;
using Core.Dtos.Payrolls.Input;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class PayrollService: IPayrollService
    {
        private readonly IUnitOfWork _unitOfWork;
        public PayrollService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Payroll>> GetPayrollByUserProfileId(Guid userId)
        {
            var userProfileSpecs = new GetUserProfileByUserIdSpecification(userId);
            var userProfile = _unitOfWork.Repository<UserProfile>().GetEntityWithSpec(userProfileSpecs);

            var specs = new GetPayrollByUserProfileIdSpecification(userProfile.Id);
            var payroll = await _unitOfWork.Repository<Payroll>().ListAsync(specs);
            return payroll;
        }

        public async Task ProcessPayrollPeriod(ProcessPayrollInputDto request, Guid userId)
        {
            if (request.EmployeeId.HasValue)
            {
                if (request.EmployeeId.Value > 0)
                {
                    var campaignAssignmentSpecs = new GetCampaignAssignmentByUserProfileIdSpecification(request.EmployeeId.Value);
                    var campaignAssignment = await _unitOfWork.Repository<CampaignAssignment>().GetEntityWithSpec(campaignAssignmentSpecs);

                    var schedulerSpecs = new GetSchedulerByUserAndCampaignSpecification(request, campaignAssignment.CampaignId);
                    var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(schedulerSpecs);

                    //Process attendance
                    var attendance = schedulers.Select(s => new
                    {
                        CampaignId = campaignAssignment.CampaignId,
                        UserProfileId = campaignAssignment.UserProfileId,
                        Points =
                            ((schedulers.Where(x => x.Date == s.Date).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage1 == s.DateMessage1).Select(s => s.DateMessage1).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage2 == s.DateMessage2).Select(s => s.DateMessage2).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage3 == s.DateMessage3).Select(s => s.DateMessage3).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage4 == s.DateMessage4).Select(s => s.DateMessage4).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage5 == s.DateMessage5).Select(s => s.DateMessage5).FirstOrDefault()) != null ? 1 : 0)
                    }).ToList();

                    //Delete current payroll record for the period
                    var deleteCurrentPayrollSpecs = new GetPayrollForPeriodByUserSpecification(campaignAssignment.UserProfileId, campaignAssignment.CampaignId, request.StartDate, request.EndDate);

                    var payrollRecord = await _unitOfWork.Repository<Payroll>().GetEntityWithSpec(deleteCurrentPayrollSpecs);

                    if (payrollRecord != null)
                    {
                        _unitOfWork.Repository<Payroll>().Delete(payrollRecord);
                        await _unitOfWork.Complete();
                    }

                    var payrollRequest = new Payroll
                    {
                        CampaignId = campaignAssignment.CampaignId,
                        UserProfileId = campaignAssignment.UserProfileId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userId.ToString(),
                        DaysActive = (attendance.Select(s => s.Points).Sum()) / 100,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IncentiveAmount = 0,
                        IncentiveCount = 0,
                        ApptSalesIncentive = 0,
                        BasePayAdjustment = 0,
                        IncentiveType = (int)IncentiveType.None,
                        IsCancelled = false,
                        OtherIncentive = 0,
                        ReferralIncentive = 0,
                        RepliesInceentive = 0,
                        SubTotal = 0,
                        Total = 0,
                        Wage = (await _unitOfWork.Repository<UserProfile>().GetByIdAsync(campaignAssignment.UserProfileId)).Wage
                    };

                    _unitOfWork.Repository<Payroll>().Add(payrollRequest);
                    await _unitOfWork.Complete();
                }
                else 
                {
                    var userProfiles = await _unitOfWork.Repository<UserProfile>().ListAllAsync();
                    foreach (var profile in userProfiles)
                    {
                        var campaignAssignmentSpecs = new GetCampaignAssignmentByUserProfileIdSpecification(profile.id);
                        var campaignAssignment = await _unitOfWork.Repository<CampaignAssignment>().GetEntityWithSpec(campaignAssignmentSpecs);
                        request.EmployeeId = profile.id;
                        var schedulerSpecs = new GetSchedulerByUserAndCampaignSpecification(request, campaignAssignment.CampaignId);
                        var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(schedulerSpecs);

                        //Process attendance
                        var attendance = schedulers.Select(s => new
                        {
                            CampaignId = campaignAssignment.CampaignId,
                            UserProfileId = campaignAssignment.UserProfileId,
                            Points =
                                ((schedulers.Where(x => x.Date == s.Date).FirstOrDefault()) != null ? 1 : 0) +
                                ((schedulers.Where(x => x.DateMessage1 == s.DateMessage1).Select(s => s.DateMessage1).FirstOrDefault()) != null ? 1 : 0) +
                                ((schedulers.Where(x => x.DateMessage2 == s.DateMessage2).Select(s => s.DateMessage2).FirstOrDefault()) != null ? 1 : 0) +
                                ((schedulers.Where(x => x.DateMessage3 == s.DateMessage3).Select(s => s.DateMessage3).FirstOrDefault()) != null ? 1 : 0) +
                                ((schedulers.Where(x => x.DateMessage4 == s.DateMessage4).Select(s => s.DateMessage4).FirstOrDefault()) != null ? 1 : 0) +
                                ((schedulers.Where(x => x.DateMessage5 == s.DateMessage5).Select(s => s.DateMessage5).FirstOrDefault()) != null ? 1 : 0)
                        }).ToList();

                        //Delete current payroll record for the period
                        var deleteCurrentPayrollSpecs = new GetPayrollForPeriodByUserSpecification(campaignAssignment.UserProfileId, campaignAssignment.CampaignId, request.StartDate, request.EndDate);

                        var payrollRecord = await _unitOfWork.Repository<Payroll>().GetEntityWithSpec(deleteCurrentPayrollSpecs);

                        if (payrollRecord != null)
                        {
                            _unitOfWork.Repository<Payroll>().Delete(payrollRecord);
                            await _unitOfWork.Complete();
                        }

                        var payrollRequest = new Payroll
                        {
                            CampaignId = campaignAssignment.CampaignId,
                            UserProfileId = campaignAssignment.UserProfileId,
                            CreatedDate = DateTime.UtcNow,
                            CreatedBy = userId.ToString(),
                            DaysActive = (attendance.Select(s => s.Points).Sum()) / 100,
                            StartDate = request.StartDate,
                            EndDate = request.EndDate,
                            IncentiveAmount = 0,
                            IncentiveCount = 0,
                            ApptSalesIncentive = 0,
                            BasePayAdjustment = 0,
                            IncentiveType = (int)IncentiveType.None,
                            IsCancelled = false,
                            OtherIncentive = 0,
                            ReferralIncentive = 0,
                            RepliesInceentive = 0,
                            SubTotal = 0,
                            Total = 0,
                            Wage = (await _unitOfWork.Repository<UserProfile>().GetByIdAsync(campaignAssignment.UserProfileId)).Wage
                        };

                        _unitOfWork.Repository<Payroll>().Add(payrollRequest);
                        await _unitOfWork.Complete();
                    }
                }
            }
            else 
            {
                var userProfiles = await _unitOfWork.Repository<UserProfile>().ListAllAsync();
                foreach (var profile in userProfiles)
                {
                    var campaignAssignmentSpecs = new GetCampaignAssignmentByUserProfileIdSpecification(profile.id);
                    var campaignAssignment = await _unitOfWork.Repository<CampaignAssignment>().GetEntityWithSpec(campaignAssignmentSpecs);
                    request.EmployeeId = profile.id;
                    var schedulerSpecs = new GetSchedulerByUserAndCampaignSpecification(request, campaignAssignment.CampaignId);
                    var schedulers = await _unitOfWork.Repository<Scheduler>().ListAsync(schedulerSpecs);

                    //Process attendance
                    var attendance = schedulers.Select(s => new
                    {
                        CampaignId = campaignAssignment.CampaignId,
                        UserProfileId = campaignAssignment.UserProfileId,
                        Points =
                            ((schedulers.Where(x => x.Date == s.Date).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage1 == s.DateMessage1).Select(s => s.DateMessage1).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage2 == s.DateMessage2).Select(s => s.DateMessage2).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage3 == s.DateMessage3).Select(s => s.DateMessage3).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage4 == s.DateMessage4).Select(s => s.DateMessage4).FirstOrDefault()) != null ? 1 : 0) +
                            ((schedulers.Where(x => x.DateMessage5 == s.DateMessage5).Select(s => s.DateMessage5).FirstOrDefault()) != null ? 1 : 0)
                    }).ToList();

                    //Delete current payroll record for the period
                    var deleteCurrentPayrollSpecs = new GetPayrollForPeriodByUserSpecification(campaignAssignment.UserProfileId, campaignAssignment.CampaignId, request.StartDate, request.EndDate);

                    var payrollRecord = await _unitOfWork.Repository<Payroll>().GetEntityWithSpec(deleteCurrentPayrollSpecs);

                    if (payrollRecord != null)
                    {
                        _unitOfWork.Repository<Payroll>().Delete(payrollRecord);
                        await _unitOfWork.Complete();
                    }

                    var payrollRequest = new Payroll
                    {
                        CampaignId = campaignAssignment.CampaignId,
                        UserProfileId = campaignAssignment.UserProfileId,
                        CreatedDate = DateTime.UtcNow,
                        CreatedBy = userId.ToString(),
                        DaysActive = (attendance.Select(s => s.Points).Sum()) / 100,
                        StartDate = request.StartDate,
                        EndDate = request.EndDate,
                        IncentiveAmount = 0,
                        IncentiveCount = 0,
                        ApptSalesIncentive = 0,
                        BasePayAdjustment = 0,
                        IncentiveType = (int)IncentiveType.None,
                        IsCancelled = false,
                        OtherIncentive = 0,
                        ReferralIncentive = 0,
                        RepliesInceentive = 0,
                        SubTotal = 0,
                        Total = 0,
                        Wage = (await _unitOfWork.Repository<UserProfile>().GetByIdAsync(campaignAssignment.UserProfileId)).Wage
                    };

                    _unitOfWork.Repository<Payroll>().Add(payrollRequest);
                    await _unitOfWork.Complete();
                }              
            }
        }
    }
}
