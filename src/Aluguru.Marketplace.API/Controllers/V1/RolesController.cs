using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Usecases.CreateUserRole;
using Aluguru.Marketplace.Register.Usecases.DeleteUserRole;
using Aluguru.Marketplace.Register.Usecases.GetUserRoles;
using Aluguru.Marketplace.Register.Usecases.GetUsersByRole;
using Aluguru.Marketplace.Register.Usecases.UpdateUserRole;
using Aluguru.Marketplace.Register.ViewModels;
using Aluguru.Marketplace.Security;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class RolesController : ApiController
    {
        public RolesController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = Policies.UserRoleReader)]
        [SwaggerOperation(Summary = "Get all user roles")]
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

        [HttpGet]
        [Route("{role}/users")]
        [Authorize(Policy = Policies.UserRoleReader)]
        [SwaggerOperation(Summary = "Get users by role", Description = "Get a users by role")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUsersByRoleCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetUsersByRole([SwaggerParameter("The user role. It can be 'User', 'Company' or 'Admin'")][FromRoute] string role)
        {
            var response = await _mediatorHandler.SendCommand<GetUsersByRoleCommand, GetUsersByRoleCommandResponse>(new GetUsersByRoleCommand(role));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.UserRoleWriter)]
        [SwaggerOperation(Summary = "Create a user role")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateUserRoleCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateRole([FromBody] CreateUserRoleViewModel viewModel)
        {
            var command = _mapper.Map<CreateUserRoleCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateUserRoleCommand, CreateUserRoleCommandResponse>(command);
            return PostResponse(nameof(CreateRole), response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserRoleWriter)]
        [SwaggerOperation(Summary = "Update a user role")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserRoleCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateRole([FromRoute] Guid id, [FromBody] UpdateUserRoleViewModel viewModel)
        {
            var command = new UpdateUserRoleCommand(id, viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateUserRoleCommand, UpdateUserRoleCommandResponse>(command);
            return PostResponse(nameof(CreateRole), response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserRoleWriter)]
        [SwaggerOperation(Summary = "Delete a user role")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateRole([FromRoute] Guid id)
        {
            var command = new DeleteUserRoleCommand(id);
            await _mediatorHandler.SendCommand<DeleteUserRoleCommand, bool>(command);
            return DeleteResponse();
        }
    }
}
