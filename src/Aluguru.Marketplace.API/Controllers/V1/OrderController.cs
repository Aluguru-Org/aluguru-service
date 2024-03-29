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
using Aluguru.Marketplace.Rent.Usecases.AddOrderItem;
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
using Aluguru.Marketplace.Rent.Usecases.RemoveOrderItem;
using Aluguru.Marketplace.Rent.Usecases.UpdateOrderItemAmount;
using Aluguru.Marketplace.Rent.Usecases.CalculateOrderFreigth;
using Aluguru.Marketplace.Rent.Usecases.OrderPreview;
using Aluguru.Marketplace.Rent.Usecases.GetRevenue;
using Aluguru.Marketplace.Rent.Usecases.GetAverageRevenue;

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
            [SwaggerParameter("If the sort order should be 'asc' (ascendant) or 'desc' (descendant), the default value is 'desc'", Required = false)][FromQuery] string sortOrder
        )
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetOrdersCommand(userId, paginateCriteria);
            var response = await _mediatorHandler.SendCommand<GetOrdersCommand, GetOrdersCommandResponse>(command);
            return GetResponse(response);
        }
        
        [HttpGet]
        [Route("company/{id}")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get all orders from company", Description = "Get a list of all orders")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetOrdersCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetOrderByCompany(
            [SwaggerParameter("The Id of a user", Required = false)][FromQuery] Guid? companyId,
            [SwaggerParameter("The page to be displayed", Required = false)][FromQuery] int? currentPage,
            [SwaggerParameter("The max number of pages that should be returned, the default value is 50", Required = false)][FromQuery] int? pageSize,
            [SwaggerParameter("If the product should be sorted by property, the default value is sort property is 'Id'", Required = false)][FromQuery] string sortBy,
            [SwaggerParameter("If the sort order should be 'asc' (ascendant) or 'desc' (descendant), the default value is 'desc'", Required = false)][FromQuery] string sortOrder
        )
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetOrdersCommand(companyId, paginateCriteria);
            var response = await _mediatorHandler.SendCommand<GetOrdersCommand, GetOrdersCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpGet]
        [Route("revenue")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get revenue in a time range")]
        public async Task<ActionResult> GetRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] Guid? companyId)
        {

            var command = new GetRevenueCommand(startDate, endDate, companyId);
            var response = await _mediatorHandler.SendCommand<GetRevenueCommand, GetRevenueCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpGet]
        [Route("average-revenue")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get the average revenue in a time range")]
        public async Task<ActionResult> GetAverageRevenue([FromQuery] DateTime startDate, [FromQuery] DateTime endDate, [FromQuery] Guid? companyId)
        {
            var command = new GetAverageRevenueCommand(startDate, endDate, companyId);
            var response = await _mediatorHandler.SendCommand<GetAverageRevenueCommand, GetAverageRevenueCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpGet]
        [Route("top-buyers")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get top buyers in a time range")]
        public async Task<ActionResult> GetTopBuyers([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok();
        }

        [HttpGet]
        [Route("top-selling-products")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get top selling products in a time range")]
        public async Task<ActionResult> GetTopSellingProducts([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok();
        }

        [HttpGet]
        [Route("best-seller-categories")]
        [Authorize(Policy = Policies.OrderReader)]
        [SwaggerOperation(Summary = "Get top selling categories in a time range")]
        public async Task<ActionResult> GetTopSellingCategories([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            return Ok();
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
        [Route("{id}/add-item")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Add Item to Order")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AddOrderItemCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> AddOrderItem([FromRoute] Guid id, [FromBody] AddOrderItemDTO dto)
        {
            var command = new AddOrderItemCommand(_aspNetUser.GetUserId(), id, dto);
            var response = await _mediatorHandler.SendCommand<AddOrderItemCommand, AddOrderItemCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpPut]
        [Route("{id}/calculate-order-freigth")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Calculate order freigth")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CalculateOrderFreigthCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CalculateOrderFreigth([FromRoute] Guid id, [FromBody] CalculateOrderFreigthDTO dto)
        {
            var command = new CalculateOrderFreigthCommand(_aspNetUser.GetUserId(), id, dto.Number, dto.Complement, dto.ZipCode);
            var response = await _mediatorHandler.SendCommand<CalculateOrderFreigthCommand, CalculateOrderFreigthCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpPut]
        [Route("{id}/update-item-amount")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Update item amount")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateOrderItemAmountCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateOrderItemAmount([FromRoute] Guid id, [FromBody] UpdateOrderItemAmountDTO dto)
        {
            var command = new UpdateOrderItemAmountCommand(_aspNetUser.GetUserId(), id, dto.OrderItemId, dto.Amount);
            var response = await _mediatorHandler.SendCommand<UpdateOrderItemAmountCommand, UpdateOrderItemAmountCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpPut]
        [Route("{id}/remove-item")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Remove Item from Order")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RemoveOrderItemCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> RemoveOrderItem([FromRoute] Guid id, [FromBody] RemoveOrderItemDTO dto)
        {
            var command = new RemoveOrderItemCommand(_aspNetUser.GetUserId(), id, dto.OrderItemId);
            var response = await _mediatorHandler.SendCommand<RemoveOrderItemCommand, RemoveOrderItemCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpPut]
        [Route("{id}/apply-voucher")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Apply voucher", Description = "Apply a voucher to the order")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplyVoucherCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> ApplyVoucher([FromRoute] Guid id, [FromBody] ApplyVoucherDTO viewModel)
        {
            var command = new ApplyVoucherCommand(id, viewModel.Code);
            var response = await _mediatorHandler.SendCommand<ApplyVoucherCommand, ApplyVoucherCommandResponse>(command);
            return PutResponse(response);
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

        [HttpPut]
        [Route("{id}/remove-voucher")]
        [Authorize(Policy = Policies.OrderWriter)]
        [SwaggerOperation(Summary = "Remove order voucher ")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteVoucherCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> RemoveVoucher([FromRoute] Guid id)
        {
            var command = new RemoveVoucherCommand(id);
            var response = await _mediatorHandler.SendCommand<RemoveVoucherCommand, DeleteVoucherCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpPost]
        [Route("preview")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Order preview")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(OrderPreviewCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] OrderPreviewDTO dto)
        {
            var command = new OrderPreviewCommand(dto);
            var response = await _mediatorHandler.SendCommand<OrderPreviewCommand, OrderPreviewCommandResponse>(command);
            return PostResponse(response);
        }   
    }
}