using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Payment.Dtos;
using Aluguru.Marketplace.Payment.Usecases.GetPayment;
using Aluguru.Marketplace.Payment.Usecases.PayOrder;
using Aluguru.Marketplace.Payment.Usecases.UpdateInvoiceStatus;
using Aluguru.Marketplace.Security;
using Aluguru.Marketplace.Security.User;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    [Authorize]
    public class PaymentController : ApiController
    {
        private readonly IAspNetUser _aspNetUser;
        public PaymentController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper, IAspNetUser aspNetUser) 
            : base(notifications, mediator, mapper)
        {
            _aspNetUser = aspNetUser;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.PaymentReader)]
        [SwaggerOperation(Summary = "Get Payment by Id")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetPaymentCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<GetPaymentCommand, GetPaymentCommandResponse>(new GetPaymentCommand(id));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("pay-order")]
        [Authorize(Policy = Policies.PaymentWriter)]
        [SwaggerOperation(Summary = "Start order payment process")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(PayOrderCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> PayOrder([FromBody] PayOrderDTO payOrderDTO)
        {
            var command = new PayOrderCommand(_aspNetUser.GetUserId(), payOrderDTO.OrderId, payOrderDTO.Installments, payOrderDTO.Token);
            var response = await _mediatorHandler.SendCommand<PayOrderCommand, PayOrderCommandResponse>(command);
            return PostResponse(nameof(Get), new { id = response.Payment?.Id } , response);
        }

        [HttpPost]
        [Route("update-invoice-status")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Update invoice status")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateInvoice([FromBody] InvoiceStatusChangedDTO invoiceStatusChanged)
        {
            var command = _mapper.Map<UpdateInvoiceStatusCommand>(invoiceStatusChanged);
            await _mediatorHandler.SendCommand<UpdateInvoiceStatusCommand, bool>(command);
            return PostResponse();
        }

    }
}
