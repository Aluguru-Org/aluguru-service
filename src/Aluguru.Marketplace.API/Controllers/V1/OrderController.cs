using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Usecases.CreateOrder;
using Aluguru.Marketplace.Rent.Usecases.DeleteOrder;
using Aluguru.Marketplace.Rent.Usecases.RemoveVoucher;
using Aluguru.Marketplace.Rent.Usecases.GetOrder;
using Aluguru.Marketplace.Rent.Usecases.GetOrders;
using Aluguru.Marketplace.Rent.Usecases.UpdateOrder;
using Aluguru.Marketplace.Rent.Dtos;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aluguru.Marketplace.Rent.Usecases.ApplyVoucher;
using Microsoft.AspNetCore.Authorization;
using Aluguru.Marketplace.Security;
using Aluguru.Marketplace.Rent.Usecases.StartOrder;
using Aluguru.Marketplace.Security.User;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    [Authorize]
    public class OrderController : ApiController
    {
        private readonly IAspNetUser _aspNetUser;
        public OrderController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper, IAspNetUser aspNetUser)
            : base(notifications, mediator, mapper)
        {
            _aspNetUser = aspNetUser;
        }
        
        [HttpGet]
        [Route("")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get all orders", Description = "Get a list of all orders")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrdersCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get(
            [SwaggerParameter("The Id of a user", Required = false)][FromQuery] Guid? userId,
            [SwaggerParameter("The page to be displayed", Required = false)][FromQuery] int? currentPage,
            [SwaggerParameter("The max number of pages that should be returned, the default value is 50", Required = false)][FromQuery] int? pageSize,
            [SwaggerParameter("If the product should be sorted by property, the default value is sort property is 'Id'", Required = false)][FromQuery] string sortBy,
            [SwaggerParameter("If the sort order should be ascendant or descendant, the default value is descendant", Required = false)][FromQuery] string sortOrder
        )
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetOrdersCommand(userId, paginateCriteria);
            var response = await _mediatorHandler.SendCommand<GetOrdersCommand, GetOrdersCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get order by id", Description = "Return the target order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetById([SwaggerParameter("The order Id", Required = true)][FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<GetOrderCommand, GetOrderCommandResponse>(new GetOrderCommand(id));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Create Order", Description = "Create a new user order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]        
        public async Task<ActionResult> Post([FromBody] CreateOrderDTO viewModel)
        {
            var command = _mapper.Map<CreateOrderCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateOrderCommand, CreateOrderCommandResponse>(command);
            return PostResponse(nameof(Get), new { id = response?.Order?.Id }, response);
        }

        [HttpPost]
        [Route("{id}/start")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Start Order", Description = "Start a order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StartOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromRoute] Guid id)
        {
            var command = new StartOrderCommand(_aspNetUser.GetUserId(), id);
            var response = await _mediatorHandler.SendCommand<StartOrderCommand, StartOrderCommandResponse>(command);
            return PostResponse(response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Update Order", Description = "Update a existing order")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] UpdateOrderDTO viewModel)
        {
            var command = _mapper.Map<UpdateOrderCommand>(viewModel);
            await _mediatorHandler.SendCommand<UpdateOrderCommand, UpdateOrderCommandResponse>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/voucher")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Apply voucher", Description = "Apply a voucher to the order")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> ApplyVoucher([FromRoute] Guid id, [FromBody] ApplyVoucherDTO viewModel)
        {
            var command = new ApplyVoucherCommand(id, viewModel.Code);
            await _mediatorHandler.SendCommand<ApplyVoucherCommand, ApplyVoucherCommandResponse>(command);
            return PutResponse();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Delete a order", Description = "Delete a existing order.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteOrderCommand, bool>(new DeleteOrderCommand(id));
            return DeleteResponse();
        }

        [HttpDelete]
        [Route("{id}/voucher")]
        [Authorize(Policy = Policies.VoucherWriter)]
        [SwaggerOperation(Summary = "Delete a voucher ", Description = "Delete a existing voucher applyied to a order.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> DeleteVoucher([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<RemoveVoucherCommand, DeleteVoucherCommandResponse>(new RemoveVoucherCommand(id));
            return DeleteResponse();
        }
    }
}