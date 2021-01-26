using AutoMapper;
using Core.Dtos.UserProfiles.Output;
using Core.Entities;

namespace scheduler_user.api.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<UserProfile, GetUserProfileOutputDto>();
            CreateMap<UserProfile, GetUserProfileDetailsOutputDto>();
        }
    }
}
