using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Newsletter.Services;
using Aluguru.Marketplace.Newsletter.ViewModels;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [AllowAnonymous]
    public class NewsletterController : ApiController
    {
        private readonly INewsletterService _newsletterService;
        public NewsletterController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper, INewsletterService newsletterService) 
            : base(notifications, mediator, mapper) 
        {
            _newsletterService = newsletterService;
        }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get all newsletter subscribers")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get()
        {
            var response = await _newsletterService.GetAllSubscribers();
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation(Summary = "Add new newsletter subscriber")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(SubscriberViewModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromQuery] SubscriberViewModel subscriberViewModel)
        {
            var response = await _newsletterService.AddSubscriber(subscriberViewModel);
            return PostResponse(nameof(Get), null, response);
        }

        [HttpDelete]
        [Route("")]
        [SwaggerOperation(Summary = "Remove subscriber from newsletter")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromQuery] SubscriberViewModel subscriberViewModel)
        {
            await _newsletterService.RemoveSubscriber(subscriberViewModel);
            return DeleteResponse();
        }
    }
}
