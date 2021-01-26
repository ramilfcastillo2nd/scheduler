using Core.Entities;

namespace Core.Specifications
{
    public class GetUserProfileByIdWithDetailsSpecification: BaseSpecification<UserProfile>
    {
        public GetUserProfileByIdWithDetailsSpecification(int userProfileId)
            :base(x =>
                (x.id == userProfileId)
            )
        {
            AddInclude(x => x.AppUser);
        }
    }
}
