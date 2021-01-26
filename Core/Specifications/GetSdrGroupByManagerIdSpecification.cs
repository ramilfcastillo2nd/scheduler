using Core.Entities;

namespace Core.Specifications
{
    public class GetSdrGroupByManagerIdSpecification: BaseSpecification<SdrGrouping>
    {
        public GetSdrGroupByManagerIdSpecification(int managerId, CommonSpecParams specParams)
            :base(x =>
                (x.ManagerId == managerId) &&
                (
                    (x.Sdr.FirstName.Contains(specParams.Search)) ||
                    (x.Sdr.LastName.Contains(specParams.Search))
                )
            )
        {
            AddInclude(s => s.Sdr);
        }
    }
}
