using Core.Dtos.CampaignPricings.Input;
using Core.Dtos.CampaignPricings.Output;
using Core.Dtos.Campaigns.Input;
using Core.Dtos.Campaigns.Output;
using Core.Dtos.Contents.Output;
using Core.Dtos.Departments.Input;
using Core.Dtos.Departments.Output;
using Core.Dtos.Payrolls.Output;
using Core.Dtos.UserProfiles.Output;
using Core.Dtos.Wages.Input;
using Core.Entities;

using AutoMapper;
using System.Linq;

namespace scheduler_core.api.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserProfile, GetUserProfileOutputDto>();
            CreateMap<UserProfile, ManagerDetailsOutputDto>();
            CreateMap<Department, GetDepartmentOutputDto>();
            CreateMap<CreateDepartmentInputDto, Department>();
            CreateMap<CreateCampaignInputDto, Campaign>();
            CreateMap<Campaign, GetCampaignOutputDto>()
                .ForMember(d => d.SdrCount, opt => opt.MapFrom(s => s.CampaignAssignments.Count()))
                .ForMember(d => d.ManagerId, opt => opt.MapFrom(s => s.UserProfileId));

            CreateMap<Campaign, GetCampaignDetailOutputDto>()
                .ForMember(d => d.Manager, opt => opt.MapFrom(s => s.UserProfile));

            CreateMap<Content, GetContentOutputDto>();
            CreateMap<CampaignPricing, GetCampaignPricingOutputDto>();
            CreateMap<CreateCampaignPricingInputDto, CampaignPricing>();
            CreateMap<Payroll, GetPayrollOutputDto>();

            CreateMap<AddWageInputDto, Wage>();
        }
    }
}
