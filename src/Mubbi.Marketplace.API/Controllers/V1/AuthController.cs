using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Usecases.LogInUser;
using Mubbi.Marketplace.Register.ViewModels;
using Swashbuckle.AspNetCore.Annotations;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    [ValidateModel]
    [ApiController]
    public class AuthController : ApiController
    {
        public AuthController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpPost]
        [Route("login")]
        [SwaggerOperation(Summary = "LogIn a user", Description = "Try to LogIn a existing user")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(LogInUserCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> LogIn([FromBody] UserLoginViewModel viewModel)
        {
            var command = new LogInUserCommand(viewModel.UserName, viewModel.Password);
            var response = await _mediatorHandler.SendCommand<LogInUserCommand, LogInUserCommandResponse>(command);
            return PostResponse(nameof(LogIn), response); 
        }
    }
}