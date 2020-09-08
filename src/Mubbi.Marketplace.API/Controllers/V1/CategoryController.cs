using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateCategory;
using Mubbi.Marketplace.Catalog.Usecases.DeleteCategory;
using Mubbi.Marketplace.Catalog.Usecases.GetCategories;
using Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Mubbi.Marketplace.Catalog.Usecases.UpdateCategory;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
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
    public class CategoryController : ApiController
    {
        public CategoryController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [Authorize(Policy = Policies.NotAnonymous)]
        [SwaggerOperation(Summary = "Get all categories", Description = "Get a list of all categories")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetAll()
        {
            var response = await _mediatorHandler.SendCommand<GetCategoriesCommand, GetCategoriesCommandResponse>(new GetCategoriesCommand());
            return GetResponse(response);
        }

        [HttpGet]
        [Route("{id}/products")]
        [Authorize(Policy = Policies.NotAnonymous)]
        [SwaggerOperation(Summary = "Get products by category", Description = "Return a list of paginated products by pagination creteria and category id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsByCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetProductsByCategory(
            [SwaggerParameter("The category Id", Required = true)][FromRoute] Guid id,
            [SwaggerParameter("The page to be displayed", Required = false)][FromQuery] int? currentPage,
            [SwaggerParameter("The max number of pages that should be returned, the default value is 50", Required = false)][FromQuery] int? pageSize,
            [SwaggerParameter("If the product should be sorted by property, the default value is sort property is 'Id'", Required = false)][FromQuery] string sortBy,
            [SwaggerParameter("If the sort order should be ascendant or descendant, the default value is descendant", Required = false)][FromQuery] string sortOrder)
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetProductsByCategoryCommand(id, paginateCriteria);

            var response = await _mediatorHandler.SendCommand<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize()]
        [SwaggerOperation(Summary = "Create a new category", Description = "Used to create a new category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateCategory([FromBody]CreateCategoryViewModel viewModel)
        {
            var command = _mapper.Map<CreateCategoryCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateCategoryCommand, CreateCategoryCommandResponse>(command);
            return PostResponse(nameof(CreateCategory), response);
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update a category", Description = "Used to update a existing category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpdateCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateCategory([FromRoute] Guid id, [FromBody] UpdateCategoryViewModel viewModel)
        {
            var command = new UpdateCategoryCommand(id, viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateCategoryCommand, UpdateCategoryCommandResponse>(command);
            return PutResponse(response);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a category", Description = "Delete a existing category and all it's sub categories. You need to inform the category Id.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteCategoryCommand, bool>(new DeleteCategoryCommand(id));
            return DeleteResponse();
        }
    }
}
