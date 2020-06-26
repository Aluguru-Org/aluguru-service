using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Shared.Communication;
using Mubbi.Marketplace.Shared.Messages.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    public abstract class ApiController : ControllerBase
    {
        protected readonly DomainNotificationHandler _notifications;
        protected readonly IMediatorHandler _mediator;

        public ApiController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator)
        {
            _notifications = notifications as DomainNotificationHandler;
            _mediator = mediator;
        }

        protected bool IsValidOperation() => !_notifications.HasNotifications();

        protected ActionResult GetResponse(object result = null)
        {
            if (IsValidOperation()) return Ok(new ApiResponse(result, "The resource has been fetched successfully"));

            return BadRequest(new ApiResponse(_notifications.GetNotificationErrors()));
        }

        protected ActionResult PostResponse(string actionName, object result = null)
        {
            if (IsValidOperation()) return CreatedAtAction(actionName, new ApiResponse(result, "The resource was successfully created."));

            return BadRequest(new ApiResponse(_notifications.GetNotificationErrors()));
        }

        protected ActionResult PutResponse()
        {
            if (IsValidOperation()) return Ok(new ApiResponse("The resource was updated successfully"));

            return BadRequest(new ApiResponse(_notifications.GetNotificationErrors()));
        }

        protected ActionResult DeleteResponse()
        {
            if (IsValidOperation()) return Ok(new ApiResponse("The resource was deleted successfully"));

            return BadRequest(new ApiResponse(_notifications.GetNotificationErrors()));
        }
    }

}
