using Core.Entities;

namespace Core.Specifications
{
    public class GetUserProfilesCountSpecification: BaseSpecification<UserProfile>
    {
        public GetUserProfilesCountSpecification(CommonSpecParams specParams)
               : base(x =>
                  ((x.FirstName.Contains(specParams.Search)) &&
                 (x.LastName.Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search)
                )
        {

        }
    }
}
