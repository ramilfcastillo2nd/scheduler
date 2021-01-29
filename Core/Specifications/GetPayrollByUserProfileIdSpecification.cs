using Core.Entities;

namespace Core.Specifications
{
    public class GetPayrollByUserProfileIdSpecification: BaseSpecification<Payroll>
    {
        public GetPayrollByUserProfileIdSpecification(int userProfileId)
            :base(x => 
                (x.UserProfileId == userProfileId)    
            )
        {
            AddOrderByDescending(x => x.StartDate);
        }
    }
}
