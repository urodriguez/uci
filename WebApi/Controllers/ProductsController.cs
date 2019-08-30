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

        public ProductsController(IProductService productService, ILoggerService loggerService) : base(productService, loggerService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("cheapest")]
        public IHttpActionResult GetCheapest(decimal maxPrice)
        {
            try
            {
                var dtos = _productService.GetCheapest(maxPrice);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
