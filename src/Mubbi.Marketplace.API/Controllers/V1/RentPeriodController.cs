using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateRentPeriod;
using Mubbi.Marketplace.Catalog.ViewModels;
using Mubbi.Marketplace.Domain;
using Mubbi.Marketplace.Infrastructure.Bus.Communication;
using Mubbi.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mubbi.Marketplace.API.Controllers.V1
{
    [Route("api/v1/periods")]
    [ValidateModel]
    [ApiController]
    public class RentPeriodController : ApiController
    {
        public CategoryController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediator, IMapper mapper)
            : base(notifications, mediator, mapper) { }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get all rent period", Description = "Get a list of all rent periods")]
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
        [SwaggerOperation(Summary = "Create a new rent period", Description = "Used to create a new rent period")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateRentPeriodCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateRentPeriod([FromBody]CreateRentPeriodViewModel viewModel)
        {
            var command = _mapper.Map<CreateRentPeriodCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateRentPeriodCommand, CreateRentPeriodCommandResponse>(command);
            return PostResponse(nameof(CreateRentPeriod), response);
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
