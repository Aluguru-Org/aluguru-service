using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Usecases.LogInUser;
using Aluguru.Marketplace.Register.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace Aluguru.Marketplace.API.Controllers.V1
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
        public async Task<ActionResult> LogIn([FromBody] LoginUserDTO loginuserDTO)
        {
            var command = new LogInUserCommand(loginuserDTO.Email, loginuserDTO.Password);
            var response = await _mediatorHandler.SendCommand<LogInUserCommand, LogInUserCommandResponse>(command);
            return PostResponse(nameof(LogIn), null, response);
        }
    }
}