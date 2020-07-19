using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Usecases.CreateUserRole;
using Mubbi.Marketplace.Register.Usecases.GetUserRoles;
using Mubbi.Marketplace.Register.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class RolesController : ApiController
    {
        public RolesController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get all UserRoles")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserRolesCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetRoles()
        {
            var response = await _mediatorHandler.SendCommand<GetUserRolesCommand, GetUserRolesCommandResponse>(new GetUserRolesCommand());
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize]
        [SwaggerOperation(Summary = "Create a UserRole")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateUserRoleCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateRole([FromBody] UserRoleViewModel viewModel)
        {
            var command = _mapper.Map<CreateUserRoleCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateUserRoleCommand, CreateUserRoleCommandResponse>(command);
            return PostResponse(nameof(CreateRole), response);
        }
    }
}
