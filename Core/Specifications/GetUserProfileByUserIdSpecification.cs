using Core.Entities;
using System;

namespace Core.Specifications
{
    public class GetUserProfileByUserIdSpecification: BaseSpecification<UserProfile>
    {
        public GetUserProfileByUserIdSpecification(Guid userId)
            :base(s => 
                (s.UserId == userId)
            )
        {

        }
    }
}
