using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace scheduler_core.api.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
    }
}
