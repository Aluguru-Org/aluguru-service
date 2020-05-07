using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn()
        {
            return null;
        }
    }
}