using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Aluguru.Marketplace.API.Controllers.V1.Attributes;
using Aluguru.Marketplace.API.Models;
using Aluguru.Marketplace.Catalog.Usecases.AddProductImage;
using Aluguru.Marketplace.Catalog.Usecases.CreateProduct;
using Aluguru.Marketplace.Catalog.Usecases.DeleteProduct;
using Aluguru.Marketplace.Catalog.Usecases.DeleteProductImage;
using Aluguru.Marketplace.Catalog.Usecases.GetProduct;
using Aluguru.Marketplace.Catalog.Usecases.GetProducts;
using Aluguru.Marketplace.Catalog.Usecases.UpdateProduct;
using Aluguru.Marketplace.Catalog.Dtos;
using Aluguru.Marketplace.Domain;
using Aluguru.Marketplace.Infrastructure.Bus.Communication;
using Aluguru.Marketplace.Infrastructure.Bus.Messages.DomainNotifications;
using Aluguru.Marketplace.Security;
using Aluguru.Marketplace.Security.User;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aluguru.Marketplace.API.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ValidateModel]
    [ApiController]
    public class ProductController : ApiController
    {
        private readonly IAspNetUser _aspNetUser;

        public ProductController(INotificationHandler<DomainNotification> notifications, IMediatorHandler mediatorHandler, IMapper mapper, IAspNetUser aspNetUser)
            : base(notifications, mediatorHandler, mapper)
        {
            _aspNetUser = aspNetUser;
        }

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get products", Description = "Return a list of products")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Get(
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
        [Route("{product}")]
        [AllowAnonymous]
        [SwaggerOperation(Summary = "Get product by uri", Description = "Return the target product")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetById([SwaggerParameter("The product uri", Required = true)][FromRoute] string product)
        {
            var response = await _mediatorHandler.SendCommand<GetProductCommand, GetProductCommandResponse>(new GetProductCommand(product));
            return GetResponse(response);
        }    

        [HttpPost]
        [Route("")]
        [Authorize(Policy = Policies.ProductWriter)]
        [SwaggerOperation(Summary = "Create Product", Description = "Create a new product in the catalog")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(CreateProductCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]        
        public async Task<ActionResult> Post([FromBody] CreateProductDTO viewModel)
        {
            var command = _mapper.Map<CreateProductCommand>(viewModel);
            var response = await _mediatorHandler.SendCommand<CreateProductCommand, CreateProductCommandResponse>(command);
            return PostResponse(nameof(Get), response != null ? new { id = response.Product.Id } : null, response);
        }

        [HttpPut]
        [Route("{id}")]
        [Authorize(Policy = Policies.ProductWriter)]
        [SwaggerOperation(Summary = "Update Product", Description = "Update a existing product in the catalog")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UpdateProduct([FromRoute] Guid id, [FromBody] UpdateProductDTO viewModel)
        {
            var command = new UpdateProductCommand(id, viewModel);
            await _mediatorHandler.SendCommand<UpdateProductCommand, UpdateProductCommandResponse>(command);
            return PutResponse();
        }

        [HttpDelete]
        [Route("{id}")]
        [Authorize(Policy = Policies.ProductWriter)]
        [SwaggerOperation(Summary = "Delete a product", Description = "Delete a existing product.")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> Delete([FromRoute] Guid id)
        {
            await _mediatorHandler.SendCommand<DeleteProductCommand, bool>(new DeleteProductCommand(id));
            return DeleteResponse();
        }

        [HttpPost]
        [Route("{id}/image")]
        [Authorize(Policy = Policies.ProductWriter)]
        [Consumes("multipart/form-data")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(AddProductImageCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> UploadImage([FromRoute] Guid id, List<IFormFile> files)
        {
            var command = new AddProductImageCommand(id, files);
            var response = await _mediatorHandler.SendCommand<AddProductImageCommand, AddProductImageCommandResponse>(command);
            return PostResponse(nameof(Get), new { id = response.Product.Id }, response);
        }

        [HttpDelete]
        [Route("{id}/image")]
        [Authorize(Policy = Policies.ProductWriter)]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DeleteProductImageCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> DeleteImage([FromRoute] Guid id, [FromBody] List<string> imageUrls)
        {
            var command = new DeleteProductImageCommand(id, imageUrls);
            await _mediatorHandler.SendCommand<DeleteProductImageCommand, DeleteProductImageCommandResponse>(command);
            return DeleteResponse();
        }
    }
}
