using Core.Entities;

namespace Core.Specifications
{
    public class GetDepartmentsCountSpecification : BaseSpecification<Department>
    {
        public GetDepartmentsCountSpecification(CommonSpecParams specParams)
            : base(x =>
                 ((x.Name.Contains(specParams.Search)) ||
                 (x.Description.ToLower().Contains(specParams.Search)) ||
                 (x.ShortName.ToLower().Contains(specParams.Search))) || string.IsNullOrEmpty(specParams.Search)
            )
        { }
    }
}
