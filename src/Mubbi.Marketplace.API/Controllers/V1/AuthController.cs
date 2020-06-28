﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Application.Usecases.LogInUser;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ValidateModel]
    [ApiController]
    public class AuthController : ApiController
    {
        public AuthController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
            : base(notifications, mediator)
        {

        }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "LogIn a user", Description = "Try to LogIn a existing user")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse))]
        public async Task<ActionResult> LogIn([FromBody] UserLoginViewModel viewModel)
        {
            var command = new LogInUserCommand(viewModel.UserName, viewModel.Password);
            var response = await _mediator.SendCommand<LogInUserCommand, LogInUserCommandResponse>(command);
            return PostResponse(nameof(LogIn), response);
        }
    }
}