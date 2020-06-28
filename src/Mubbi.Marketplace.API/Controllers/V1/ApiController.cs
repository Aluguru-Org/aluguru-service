using MediatR;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;

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

        protected ActionResult GetResponse(object data = null)
        {
            if (IsValidOperation()) return Ok(new ApiResponse(true, "The resource has been fetched successfully", data));

            return BadRequest(new ApiResponse(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult PostResponse(string actionName, object data = null)
        {
            if (IsValidOperation()) return CreatedAtAction(actionName, new ApiResponse(true, "The resource was successfully created.", data));

            return BadRequest(new ApiResponse(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult PutResponse()
        {
            if (IsValidOperation()) return Ok(new ApiResponse(true, "The resource was updated successfully"));

            return BadRequest(new ApiResponse(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult DeleteResponse()
        {
            if (IsValidOperation()) return Ok(new ApiResponse(true, "The resource was deleted successfully"));

            return BadRequest(new ApiResponse(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }
    }

}
