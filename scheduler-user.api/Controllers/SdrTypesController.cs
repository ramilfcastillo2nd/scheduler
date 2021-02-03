using Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_user.api.Controllers
{
    public class SdrTypesController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetSdrTypes()
        {
            try
            {
                var sdrTypes = Enum.GetValues(typeof(SdrType))
                     .Cast<SdrType>()
                     .Select(v => new {
                         Id = (int)v,
                         Value = v.ToString()
                     }).ToList();

                return Ok(sdrTypes);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
