using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Mubbi.Marketplace.Register.Usecases.CreateRole;
using Mubbi.Marketplace.Register.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CreateRole([FromBody] UserRoleViewModel viewModel)
        {
            var command = new CreateRoleCommand(viewModel.Name);
            var response = await _mediatorHandler.SendCommand<CreateRoleCommand, CreateRoleCommandResponse>(command);
            return PostResponse(nameof(CreateRole), response);
        }
    }
}
