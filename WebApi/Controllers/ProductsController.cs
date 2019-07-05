using System;
using System.Linq;
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
        public IHttpActionResult Get(int id)
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
            var productDtoCreated = _productService.Create(productDto);
            return Ok(productDtoCreated);
        }

        [HttpPut]
        public IHttpActionResult Update(int id, [FromBody] ProductDto productDto)
        {
            var productDtoUpdated = _productService.Update(productDto);

            return Ok(productDtoUpdated);
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            if (id <= 0) return BadRequest("Not a valid id");

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

        [HttpDelete]
        public IHttpActionResult DeleteRange(string ids)
        {
            try
            {
                var idsList = ids.Split(';').Select(id => Convert.ToInt32(id));

                _productService.DeleteRange(idsList);

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest("ids must be only positive integer numbers");
            }
        }
    }
}
