using Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace scheduler_core.api.Controllers
{
    public class CampaignTypesController : BaseApiController
    {
        public CampaignTypesController()
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetCampaignTypes()
        {
            try
            {
               var campaignTypes = Enum.GetValues(typeof(CampaignType))
                    .Cast<CampaignType>()
                    .Select(v => new {
                        Id = (int)v,
                        Value = v.ToString()
                    }).ToList();

                return Ok(campaignTypes);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
