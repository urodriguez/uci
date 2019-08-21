using System;
using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    public class ProductsController : CrudController<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) : base(productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("api/products/cheapest")]
        public IHttpActionResult GetCheapest()
        {
            //TODO: see https://www.tutorialsteacher.com/webapi/web-api-routing
            //TODO: use maxPrice
            try
            {
                var dtos = _productService.GetCheapest(100);
                return Ok(dtos);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
