using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.CreateCategory;
using Aluguru.Marketplace.Catalog.Usecases.DeleteCategory;
using Aluguru.Marketplace.Catalog.Usecases.GetCategories;
using Aluguru.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Aluguru.Marketplace.Catalog.Usecases.UpdateCategory;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Security;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Aluguru.Marketplace.Catalog.Usecases.AddCategoryImage;
using Aluguru.Marketplace.Catalog.Usecases.DeleteCategoryImage;
using Aluguru.Marketplace.Catalog.Usecases.GetCategory;

namespace Aluguru.Marketplace.API.Controllers.V1
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
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get all categories", Description = "Get a list of all categories")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get()
        {
            var response = await _mediatorHandler.SendCommand<GetCategoriesCommand, GetCategoriesCommandResponse>(new GetCategoriesCommand());
            return GetResponse(response);
        }

        [HttpGet]
        [Route("{category}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get a category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetCategoriesCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetById([SwaggerParameter("The category uri", Required = true)][FromRoute] string category)
        {
            var response = await _mediatorHandler.SendCommand<GetCategoryCommand, GetCategoryCommandResponse>(new GetCategoryCommand(category));
            return GetResponse(response);
        }

        [HttpGet]
        [Route("products")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get products by category", Description = "Return a list of paginated products by pagination creteria and category id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsByCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetProductsByCategories(
            [SwaggerParameter("The categories uri", Required = true)][FromQuery] List<string> categoriesUri,
            [SwaggerParameter("The page to be displayed", Required = false)][FromQuery] int? currentPage,
            [SwaggerParameter("The max number of pages that should be returned, the default value is 50", Required = false)][FromQuery] int? pageSize,
            [SwaggerParameter("If the product should be sorted by property, the default value is sort property is 'Id'", Required = false)][FromQuery] string sortBy,
            [SwaggerParameter("If the sort order should be ascendant or descendant, the default value is descendant", Required = false)][FromQuery] string sortOrder)
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetProductsByCategoryCommand(categoriesUri, paginateCriteria);

            var response = await _mediatorHandler.SendCommand<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.CategoryWriter)]
        [SwaggerOperation(Summary = "Create a new category", Description = "Used to create a new category")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Post([FromBody]CreateCategoryDTO viewModel)
        {
            var command = _mapper.Map<CreateCategoryCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateCategoryCommand, CreateCategoryCommandResponse>(command);
            return PostResponse(nameof(Get), new { category = response?.Category?.Name }, response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.CategoryWriter)]
        [SwaggerOperation(Summary = "Update a category", Description = "Used to update a existing category")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Put([FromRoute] Guid id, [FromBody] UpdateCategoryDTO viewModel)
        {
            var command = new UpdateCategoryCommand(id, viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateCategoryCommand, UpdateCategoryCommandResponse>(command);
            return PutResponse();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.CategoryWriter)]
        [SwaggerOperation(Summary = "Delete a category", Description = "Delete a existing category and all it's sub categories. You need to inform the category Id.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteCategoryCommand, bool>(new DeleteCategoryCommand(id));
            return DeleteResponse();
        }

        [HttpPost]
        [Route("{id}/image")]
        [Authorize(Policy = Policies.CategoryWriter)]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UpdateCategoryImageCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UploadImage([FromRoute] Guid id, IFormFile file)
        {
            var command = new UpdateCategoryImageCommand(id, file);
            var response = await _mediatorHandler.SendCommand<UpdateCategoryImageCommand, UpdateCategoryImageCommandResponse>(command);
            return PostResponse(nameof(Get), new { category = response.Category.Name }, response);
        }

        [HttpDelete]
        [Route("{id}/image")]
        [Authorize(Policy = Policies.CategoryWriter)]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UploadImage([FromRoute] Guid id)
        {
            var command = new DeleteCategoryImageCommand(id);
            await _mediatorHandler.SendCommand<DeleteCategoryImageCommand, DeleteCategoryImageCommandResponse>(command);
            return DeleteResponse();
        }
    }
}
