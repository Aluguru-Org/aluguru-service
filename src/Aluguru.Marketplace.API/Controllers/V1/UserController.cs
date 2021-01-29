using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Register.Usecases.CreateUser;
using Aluguru.Marketplace.Register.Usecases.DeleteUser;
using Aluguru.Marketplace.Register.Usecases.GetUserById;
using Aluguru.Marketplace.Register.Usecases.UpadeUserName;
using Aluguru.Marketplace.Register.Usecases.UpdateUserPassword;
using Aluguru.Marketplace.Register.Dtos;
using Aluguru.Marketplace.Security;
using Aluguru.Marketplace.Security.User;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aluguru.Marketplace.Register.Usecases.ActivateUser;
using Aluguru.Marketplace.Register.Usecases.UpadeUserDocument;
using Aluguru.Marketplace.Register.Usecases.UpadeUserAddress;
using Aluguru.Marketplace.Register.Usecases.UpadeUserContact;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class UserController : ApiController
    {
        private readonly IAspNetUser _aspNetUser;
        public UserController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler, IMapper mapper, IAspNetUser aspNetUser)
            : base(notifications, mediatorHandler, mapper) 
        {
            _aspNetUser = aspNetUser;
        }

        [HttpGet]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserReader)]
        [SwaggerOperation(Summary = "Get user by id", Description = "Get a user by Id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetUserByIdCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] UserRegistrationDTO viewModel)
        {
            var command = _mapper.Map<CreateUserCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateUserCommand, CreateUserCommandResponse>(command);
            return PostResponse(nameof(Get), response != null ? new { id = response.User.Id } : null, response);
        }

        [HttpPut]
        [Route("{id}/name")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update user name")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateUserName(
            [FromRoute] Guid id,
            [FromBody] UpdateUserNameDTO dto)
        {
            var command = new UpdateUserNameCommand(id, dto.FullName);
            await _mediatorHandler.SendCommand<UpdateUserNameCommand, bool>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/document")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update user document")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateUserDocument(
            [FromRoute] Guid id,
            [FromBody] UpdateUserDocumentDTO dto)
        {
            var command = new UpdateUserDocumentCommand(id, dto.Number, dto.DocumentType);
            await _mediatorHandler.SendCommand<UpdateUserDocumentCommand, bool>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/contact")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update user contact")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateUserContact(
            [FromRoute] Guid id,
            [FromBody] UpdateUserContactDTO dto)
        {
            var command = new UpdateUserContactCommand(id, dto.Name, dto.PhoneNumber, dto.Email);
            await _mediatorHandler.SendCommand<UpdateUserContactCommand, bool>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/address")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update user address")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateUserAddress(
            [FromRoute] Guid id,
            [FromBody] UpdateUserAddressDTO dto)
        {
            var command = new UpdateUserAddressCommand(id, dto.Street, dto.Number, dto.Neighborhood, dto.City, dto.State, dto.Country, dto.ZipCode, dto.Complement);
            await _mediatorHandler.SendCommand<UpdateUserAddressCommand, bool>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/password")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Update a user password", Description = "Update a existing user password.")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdatePassword([FromRoute] Guid id, [FromBody] UpdateUserPasswordDTO viewModel)
        {
            var command = new UpdateUserPasswordCommand(_aspNetUser.GetUserId(), id, viewModel.Password);
            await _mediatorHandler.SendCommand<UpdateUserPasswordCommand, bool>(command);
            return PutResponse();
        }

        [HttpPut]
        [Route("{id}/activate")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Activate a user", Description = "Update a existing user password.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> ActivateUser([FromRoute] Guid id,
            [SwaggerParameter("The user activation hash", Required = true)][FromQuery] string activationHash)
        {
            var command = new ActivateUserCommand(id, activationHash);
            await _mediatorHandler.SendCommand<ActivateUserCommand, bool>(command);
            return PutResponse();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.UserWriter)]
        [SwaggerOperation(Summary = "Delete a user", Description = "Delete a existing user. You need to inform the user Id.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteUserCommand, bool>(new DeleteUserCommand(id));
            return DeleteResponse();
        }
    }
}
