using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Mubbi.Marketplace.Shared.Communication;
using Mubbi.Marketplace.Shared.Messages.Notifications;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ApiController
    {
        public AuthController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
            : base(notifications, mediator)
        {

        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> LogIn([FromBody] UserLoginViewModel viewModel)
        {

            return PostResponse(nameof(LogIn), result);
        }
    }
}