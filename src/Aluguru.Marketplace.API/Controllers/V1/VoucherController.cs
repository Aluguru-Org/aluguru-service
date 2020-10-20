using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Rent.Usecases.CreateVoucher;
using Aluguru.Marketplace.Rent.Usecases.DeleteVoucher;
using Aluguru.Marketplace.Rent.Usecases.GetVouchers;
using Aluguru.Marketplace.Rent.ViewModels;
using Aluguru.Marketplace.Security;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class VoucherController : ApiController
    {
        public VoucherController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = Policies.VoucherReader)]
        [SwaggerOperation(Summary = "Get all vouchers", Description = "Get a list of all vouchers")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetVouchersCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetAll()
        {
            var response = await _mediatorHandler.SendCommand<GetVouchersCommand, GetVouchersCommandResponse>(new GetVouchersCommand());
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.VoucherWriter)]
        [SwaggerOperation(Summary = "Create a new voucher")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateVoucherCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateVoucher([FromBody]CreateVoucherViewModel viewModel)
        {
            var command = _mapper.Map<CreateVoucherCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateVoucherCommand, CreateVoucherCommandResponse>(command);
            return PostResponse(nameof(CreateVoucher), response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.VoucherWriter)]
        [SwaggerOperation(Summary = "Delete a voucher", Description = "Delete a voucher if is not used by order, otherwise the request will return a error")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] string code)
        {
            await _mediatorHandler.SendCommand<DeleteVoucherCommand, bool>(new DeleteVoucherCommand(code));
            return DeleteResponse();
        }
    }
}
