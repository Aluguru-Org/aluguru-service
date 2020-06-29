using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Application.Usecases.CreateUser;
using Mubbi.Marketplace.Register.Application.Usecases.GetUserById;
using Mubbi.Marketplace.Register.Application.ViewModels;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class UserController : ApiController
    {
        public UserController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
            : base(notifications, mediator)
        {

        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get user by id", Description = "Get a user by Id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<GetUserByIdCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get([FromRoute] Guid id)
        {
            var command = new GetUserByIdCommand(id);
            var response = await _mediatorHandler.SendCommand<GetUserByIdCommand, GetUserByIdCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation(Summary = "Create a user", Description = "Create a new user. You need to inform the name, role, document and address")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(ApiResponse<CreateUserCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody] UserRegistrationViewModel viewModel)
        {
            var command = new CreateUserCommand(
                viewModel.FullName, 
                viewModel.Password, 
                viewModel.Email, 
                viewModel.Role,
                viewModel.Document.DocumentType, 
                viewModel.Document.Number,
                viewModel.Address.Number, 
                viewModel.Address.State, 
                viewModel.Address.Neighborhood, 
                viewModel.Address.City, 
                viewModel.Address.State, 
                viewModel.Address.Country, 
                viewModel.Address.ZipCode);

            var response = await _mediatorHandler.SendCommand<CreateUserCommand, CreateUserCommandResponse>(command);

            return PostResponse(nameof(Post), response);
        }
    }
}
