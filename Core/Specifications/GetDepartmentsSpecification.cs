using Core.Entities;

namespace Core.Specifications
{
    public class GetDepartmentsSpecification: BaseSpecification<Department>
    {
        public GetDepartmentsSpecification(CommonSpecParams specParams)
            :base(x => 
                ((x.Name.Contains(specParams.Search)) ||
                (x.Description.ToLower().Contains(specParams.Search)) ||
                (x.ShortName.ToLower().Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search)
            )
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);

            if (!string.IsNullOrEmpty(specParams.Sort))
            {
                switch (specParams.Sort)
                {
                    case "nameAsc":
                        AddOrderBy(p => p.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(p => p.Name);
                        break;
                    default:
                        AddOrderBy(p => p.Name);
                        break;
                }
            }
        }
    }
}
