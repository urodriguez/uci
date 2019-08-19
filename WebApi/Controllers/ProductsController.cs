using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    public class ProductsController : CrudController<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) : base(productService)
        {
        }
    }
}
