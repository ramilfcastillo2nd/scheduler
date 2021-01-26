using AutoMapper;
using Core.Dtos.Campaigns.Output;
using Core.Dtos.Schedulers.Input;
using Core.Dtos.Schedulers.Output;
using Core.Entities;

namespace scheduler.api.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<SchedulerFileInputDto, Scheduler>()
                .ForMember(d => d.City, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Location)?string.Empty: s.Location.Trim().Split()[0].Replace(",", string.Empty)))
                .ForMember(d => d.State, opt => opt.MapFrom(s => string.IsNullOrEmpty(s.Location) || (s.Location.Trim().Split()).Length <= 1? string.Empty : s.Location.Trim().Split()[1].Replace(",", string.Empty)));

            CreateMap<Scheduler, SchedulerOutputDto>();
            CreateMap<UpdateSchedulerInputDto, Scheduler>();
            CreateMap<Campaign, GetCampaignForSchedulerOutputDto>();
            CreateMap<Content, ContentOutputDto>();
        }
    }
}
