using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("")]
        public ActionResult Get()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Somethign unexpected happend.");
        }
    }
}
