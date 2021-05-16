using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Newsletter.Services;
using Aluguru.Marketplace.Notification.Dtos;
using Aluguru.Marketplace.Notification.Usecases.SendAccountActivationEmail;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class NotificationController : ApiController
    {
        public NotificationController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper)
        {
        }

        [HttpPost]
        [Route("resend-activation-email")]
        [SwaggerOperation(Summary = "Resend activation e-mail")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(bool))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] ResendActivationEmailDTO dto)
        {
            var command = new SendAccountActivationEmailCommand(dto.UserId);
            await _mediatorHandler.SendCommand<SendAccountActivationEmailCommand, bool>(command);
            return PostResponse();
        }
    }
}
