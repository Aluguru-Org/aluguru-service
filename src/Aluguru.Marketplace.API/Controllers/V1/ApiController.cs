﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using System.Collections.Generic;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    public abstract class ApiController : ControllerBase
    {
        protected readonly DomainNotificationHandler _notifications;
        protected readonly IMediatorHandler _mediatorHandler;
        protected readonly IMapper _mapper;

        protected ApiController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
        {
            _notifications = notifications as DomainNotificationHandler;
            _mediatorHandler = mediator;
            _mapper = mapper;
        }

        protected bool IsValidOperation() => !_notifications.HasNotifications();

        protected ActionResult GetResponse<T>(T data = null) where T : class
        {
            if (IsValidOperation()) return Ok(new ApiResponse<T>(true, "The resource has been fetched successfully", data));

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult PostResponse<T>(string actionName, T data = null, bool isCreation = true) where T : class
        {
            if (IsValidOperation())
            {
                if (isCreation)
                    return CreatedAtAction(actionName, new ApiResponse<T>(true, "The resource was successfully created.", data));
                else
                    return Ok(new ApiResponse<T>(true, "The resource has been fetched successfully", data));
            }

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult PutResponse()
        {
            if (IsValidOperation()) return Ok(new ApiResponse(true, "The resource was updated successfully"));

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult PutResponse<T>(T data = null) where T : class
        {
            if (IsValidOperation()) return Ok(new ApiResponse<T>(true, "The resource was updated successfully", data));

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult DeleteResponse()
        {
            if (IsValidOperation())
                return Ok(new ApiResponse(true, "The resource was deleted successfully"));

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }

        protected ActionResult DeleteResponse<T>(T data = null) where T : class
        {
            if (IsValidOperation())
                return Ok(new ApiResponse<T>(true, "The resource was deleted successfully", data));

            return BadRequest(new ApiResponse<List<string>>(false, "The server was not able to process the request", _notifications.GetNotificationErrors()));
        }
    }

}