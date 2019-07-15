using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Application.Contracts;
using Application.Dtos;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductsController : ApiController
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var products = _productService.GetAll();
                return Ok(products);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri] Guid id)
        {
            try
            {
                var product = _productService.GetById(id);
                if (product == null) return NotFound();

                return Ok(product);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] ProductDto productDto)
        {
            try
            {
                var productCreatedId = _productService.Create(productDto);
                return Ok(productCreatedId);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        public IHttpActionResult Update([FromUri] Guid id, [FromBody] ProductDto productDto)
        {
            try
            {
                productDto.Id = id;
                _productService.Update(productDto);

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] Guid id)
        {
            try
            {
                _productService.Delete(id);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}
