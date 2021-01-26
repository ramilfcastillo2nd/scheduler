using Core.Entities;

namespace Core.Specifications
{
    public class GetUserProfilesByDepartmentSpecification: BaseSpecification<UserProfile>
    {
        public GetUserProfilesByDepartmentSpecification(int departmentId, CommonSpecParams specParams)
            :base(x => 
                (x.DepartmentId == departmentId) &&
                (((x.FirstName.Contains(specParams.Search)) &&
                (x.LastName.Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search))
                )
        {
            AddOrderBy(x => x.FirstName);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.FirstName);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.FirstName);
                        break;
                    default:
                        AddOrderBy(p => p.FirstName);
                        break;
                }
            }
        }
    }
}
