using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Rent.Usecases.CreateOrder;
using Mubbi.Marketplace.Rent.Usecases.DeleteOrder;
using Mubbi.Marketplace.Rent.Usecases.RemoveVoucher;
using Mubbi.Marketplace.Rent.Usecases.GetOrder;
using Mubbi.Marketplace.Rent.Usecases.GetOrders;
using Mubbi.Marketplace.Rent.Usecases.UpdateOrder;
using Mubbi.Marketplace.Rent.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mubbi.Marketplace.Rent.Usecases.ApplyVoucher;
using Microsoft.AspNetCore.Authorization;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    [Authorize]
    public class OrderController : ApiController
    {
        public OrderController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }
        
        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get all orders", Description = "Get a list of all orders")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrdersCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetALl(
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
        [SwaggerOperation(Summary = "Get order by id", Description = "Return the target order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetOrderById([SwaggerParameter("The order Id", Required = true)][FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<GetOrderCommand, GetOrderCommandResponse>(new GetOrderCommand(id));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation(Summary = "Create Order", Description = "Create a new user order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]        
        public async Task<ActionResult> CreateProduct([FromBody] CreateOrderViewModel viewModel)
        {
            var command = _mapper.Map<CreateOrderCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateOrderCommand, CreateOrderCommandResponse>(command);
            return PostResponse(nameof(CreateProduct), response);
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update Order", Description = "Update a existing order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateOrder([FromRoute] Guid id, [FromBody] UpdateOrderViewModel viewModel)
        {
            var command = _mapper.Map<UpdateOrderCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateOrderCommand, UpdateOrderCommandResponse>(command);
            return PostResponse(nameof(CreateProduct), response);
        }

        [HttpPut]
        [Route("{id}/voucher")]
        [SwaggerOperation(Summary = "Apply voucher", Description = "Apply a voucher to the order")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplyVoucherCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> ApplyVoucher([FromRoute] Guid id, [FromBody] ApplyVoucherViewModel viewModel)
        {
            var command = new ApplyVoucherCommand(id, viewModel.Code);
            var response = await _mediatorHandler.SendCommand<ApplyVoucherCommand, ApplyVoucherCommandResponse>(command);
            return PostResponse(nameof(CreateProduct), response);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a order", Description = "Delete a existing order.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteOrderCommand, bool>(new DeleteOrderCommand(id));
            return DeleteResponse();
        }

        [HttpDelete]
        [Route("{id}/voucher")]
        [SwaggerOperation(Summary = "Delete a voucher ", Description = "Delete a existing voucher applyied to a order.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteVoucherCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> DeleteVoucher([FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<RemoveVoucherCommand, DeleteVoucherCommandResponse>(new RemoveVoucherCommand(id));
            return DeleteResponse(response);
        }
    }
}