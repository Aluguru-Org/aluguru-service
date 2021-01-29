using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Aluguru.Marketplace.Catalog.Usecases.DeleteRentPeriod;
using Aluguru.Marketplace.Catalog.Usecases.GetRentPeriods;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Security;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/rent-period")]
    [ValidateModel]
    [ApiController]
    public class RentPeriodController : ApiController
    {
        public RentPeriodController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all rent period", Description = "Get a list of all rent periods")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetRentPeriodsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get()
        {
            var response = await _mediatorHandler.SendCommand<GetRentPeriodsCommand, GetRentPeriodsCommandResponse>(new GetRentPeriodsCommand());
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.RentPeriodWriter)]
        [SwaggerOperation(Summary = "Create a new rent period", Description = "Used to create a new rent period")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateRentPeriodCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] CreateRentPeriodDTO viewModel)
        {
            var command = _mapper.Map<CreateRentPeriodCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateRentPeriodCommand, CreateRentPeriodCommandResponse>(command);
            return PostResponse(nameof(Get), new { id = response.RentPeriod.Id }, response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.RentPeriodWriter)]
        [SwaggerOperation(Summary = "Delete a rent period", Description = "Delete a existing rent period")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteRentPeriodCommand, bool>(new DeleteRentPeriodCommand(id));
            return DeleteResponse();
        }
    }
}
