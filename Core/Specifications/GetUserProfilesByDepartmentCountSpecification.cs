using Core.Entities;
using System;

namespace Core.Specifications
{
    public class GetUserProfilesByDepartmentCountSpecification: BaseSpecification<UserProfile>
    {
        public GetUserProfilesByDepartmentCountSpecification(int departmentId, CommonSpecParams specParams)
            : base(x =>
                 (x.DepartmentId == departmentId) &&
                 (((x.FirstName.Contains(specParams.Search)) &&
                 (x.LastName.Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search))
                )

        {

        }
    }
}
