using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/products")]
    public class ProductsController : CrudController<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService, ILogService loggerService, IInventAppContext inventAppContext) : base(productService, loggerService, inventAppContext)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("cheapest")]
        public async Task<IHttpActionResult> GetCheapest(decimal maxPrice) => await ExecuteAsync(async () => await _productService.GetCheapestAsync(maxPrice));
    }
}
