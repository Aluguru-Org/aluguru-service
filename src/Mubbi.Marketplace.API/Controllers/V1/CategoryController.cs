﻿using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Application.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Application.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Application.Usecases.GetCategories;
using Mubbi.Marketplace.Catalog.Application.Usecases.GetProducts;
using Mubbi.Marketplace.Catalog.Application.ViewModels;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
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
    public class CategoryController : ApiController
    {
        public CategoryController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get all categories", Description = "Get a list of all categories")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<GetAllCategoriesCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetAll()
        {
            var response = await _mediatorHandler.SendCommand<GetAllCategoriesCommand, GetAllCategoriesCommandResponse>(new GetAllCategoriesCommand());
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [SwaggerOperation(Summary = "Create a new category", Description = "Used to create a new main category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApiResponse<CreateCategoryCommandResponse>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateCategory([FromBody]CreateCategoryViewModel viewModel)
        {
            var command = _mapper.Map<CreateCategoryCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateCategoryCommand, CreateCategoryCommandResponse>(command);
            return PostResponse(nameof(CreateCategory), response);
        }
    }
}
