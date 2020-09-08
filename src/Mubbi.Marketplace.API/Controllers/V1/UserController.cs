using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Usecases.CreateUser;
using Mubbi.Marketplace.Register.Usecases.DeleteUser;
using Mubbi.Marketplace.Register.Usecases.GetUserById;
using Mubbi.Marketplace.Register.Usecases.UpadeUser;
using Mubbi.Marketplace.Register.ViewModels;
using Mubbi.Marketplace.Security;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class UserController : ApiController
    {
        public UserController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler, IMapper mapper)
            : base(notifications, mediatorHandler, mapper) { }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserReader)]
        [SwaggerOperation(Summary = "Get user by id", Description = "Get a user by Id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<GetUserByIdCommand, GetUserByIdCommandResponse>(new GetUserByIdCommand(id));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Create a user", Description = "Create a new user. You need to inform the name, role, document and address")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateUserCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] UserRegistrationViewModel viewModel)
        {
            var command = _mapper.Map<CreateUserCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateUserCommand, CreateUserCommandResponse>(command);
            return PostResponse(nameof(Post), response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update a user", Description = "Update a existing user. You need to inform may update the name, document and address")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateUserCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Put(
            [FromRoute] Guid id,
            [FromBody] UpdateUserViewModel viewModel)
        {            
            var command = _mapper.Map<UpdateUserCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateUserCommand, UpdateUserCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Delete a user", Description = "Delete a existing user. You need to inform the user Id.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteUserCommand, bool>(new DeleteUserCommand(id));
            return DeleteResponse();
        }
    }
}
