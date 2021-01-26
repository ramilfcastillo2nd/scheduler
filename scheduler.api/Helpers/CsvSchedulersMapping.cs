using Core.Dtos.Schedulers.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TinyCsvParser.Mapping;

namespace scheduler.api.Helpers
{
    public class CsvSchedulersMapping : CsvMapping<SchedulerFileInputDto>
    {
        public CsvSchedulersMapping()
            :base()
        {
            MapProperty(0, x => x.FirstName);
            MapProperty(1, x => x.LastName);
            MapProperty(2, x => x.JobTitle);
            MapProperty(3, x => x.CompanyName);
            MapProperty(4, x => x.Location);
            MapProperty(5, x => x.LinkedInUrl);
        }
    }
}
