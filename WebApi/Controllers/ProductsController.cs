using Application.Contracts.Services;
using Application.Dtos;
using Domain.Aggregates;

namespace WebApi.Controllers
{
    
    public class ProductsController : CrudController<ProductDto, Product>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService) : base(productService)
        {
        }
    }
}
