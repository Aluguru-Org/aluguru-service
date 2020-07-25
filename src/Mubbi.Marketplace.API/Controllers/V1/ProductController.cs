using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Usecases.DeleteProduct;
using Mubbi.Marketplace.Catalog.Usecases.GetProduct;
using Mubbi.Marketplace.Catalog.Usecases.GetProducts;
using Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory;
using Mubbi.Marketplace.Catalog.Usecases.UpdateProduct;
using Mubbi.Marketplace.Catalog.ViewModels;
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
    public class ProductController : ApiController
    {
        public ProductController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler, IMapper mapper)
            : base(notifications, mediatorHandler, mapper) { }

        [HttpGet]
        [Route("")]
        [SwaggerOperation(Summary = "Get products", Description = "Return a list of products")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetProducts(
            [SwaggerParameter("The Id of a user", Required = false)][FromQuery] Guid? userId,
            [SwaggerParameter("The page to be displayed", Required = false)][FromQuery] int? currentPage,
            [SwaggerParameter("The max number of pages that should be returned, the default value is 50", Required = false)][FromQuery] int? pageSize,
            [SwaggerParameter("If the product should be sorted by property, the default value is sort property is 'Id'", Required = false)][FromQuery] string sortBy,
            [SwaggerParameter("If the sort order should be ascendant or descendant, the default value is descendant", Required = false)][FromQuery] string sortOrder)
        {
            var paginateCriteria = new PaginateCriteria(currentPage, pageSize, sortBy, sortOrder);
            var command = new GetProductsCommand(userId, paginateCriteria);
            var response = await _mediatorHandler.SendCommand<GetProductsCommand, GetProductsCommandResponse>(command);
            return GetResponse(response);
        }

        [HttpGet]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get product by id", Description = "Return the target product")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetProductById([SwaggerParameter("The product Id", Required = true)][FromRoute] Guid id)
        {
            var response = await _mediatorHandler.SendCommand<GetProductCommand, GetProductCommandResponse>(new GetProductCommand(id));
            return GetResponse(response);
        }    

        [HttpPost]
        [Route("")]
        [SwaggerOperation(Summary = "Create Product", Description = "Create a new product in the catalog")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateProductCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]        
        public async Task<ActionResult> CreateProduct([FromBody] CreateProductViewModel viewModel)
        {
            var command = _mapper.Map<CreateProductCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateProductCommand, CreateProductCommandResponse>(command);
            return PostResponse(nameof(CreateProduct), response);
        }

        [HttpPut]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Update Product", Description = "Update a existing product in the catalog")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CreateProductCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> CreateProduct([FromRoute] Guid id, [FromBody] UpdateProductViewModel viewModel)
        {
            var command = new UpdateProductCommand(id, viewModel);
            var response = await _mediatorHandler.SendCommand<UpdateProductCommand, UpdateProductCommandResponse>(command);
            return PostResponse(nameof(CreateProduct), response);
        }

        [HttpDelete]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete a product", Description = "Delete a existing product.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteProductCommand, bool>(new DeleteProductCommand(id));
            return DeleteResponse();
        }
    }
}
