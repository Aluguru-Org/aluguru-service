using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("")]
        [SwaggerOperation(Summary = "Default error route", Description = "Show the error")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ObjectResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ObjectResult))]
        public ActionResult Get()
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Somethign unexpected happend.");
        }
    }
}
