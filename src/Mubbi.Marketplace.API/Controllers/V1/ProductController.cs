using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mubbi.Marketplace.API.Controllers.V1.Attributes;
using Mubbi.Marketplace.API.Models;
using Mubbi.Marketplace.Catalog.Usecases.CreateProduct;
using Mubbi.Marketplace.Catalog.Usecases.GetProduct;
using Mubbi.Marketplace.Catalog.Usecases.GetProducts;
using Mubbi.Marketplace.Catalog.Usecases.GetProductsByCategory;
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
        [Route("paginate")]
        [SwaggerOperation(Summary = "Get products", Description = "Return a list of paginated products by pagination creteria")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]
        public async Task<ActionResult> GetProducts([SwaggerParameter("The pagination criteria", Required = true)][FromBody] PaginateCriteria paginateCriteria)
        {
            var response = await _mediatorHandler.SendCommand<GetProductsCommand, GetProductsCommandResponse> (new GetProductsCommand(paginateCriteria));
            return GetResponse(response);
        }

        [HttpPost]
        [Route("category/{categoryId}/paginate")]
        [SwaggerOperation(Summary = "Get products by category", Description = "Return a list of paginated products by pagination creteria and category id")]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(GetProductsByCategoryCommandResponse))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ApiResponse<List<string>>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ApiResponse<List<string>>))]        
        public async Task<ActionResult> GetProducts(
            [SwaggerParameter("The category Id", Required = true)][FromRoute] Guid categoryId,
            [SwaggerParameter("The pagination criteria", Required = true)][FromBody] PaginateCriteria paginateCriteria)
        {
            var response = await _mediatorHandler.SendCommand<GetProductsByCategoryCommand, GetProductsByCategoryCommandResponse>(
                new GetProductsByCategoryCommand(categoryId, paginateCriteria));
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
    }
}
