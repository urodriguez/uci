using System;
using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/products")]
    public class ProductsController : CrudController<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService, ILogService loggerService) : base(productService, loggerService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("cheapest")]
        public IHttpActionResult GetCheapest(decimal maxPrice) => Execute(() => _productService.GetCheapest(maxPrice));
    }
}
