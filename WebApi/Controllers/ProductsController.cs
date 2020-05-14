﻿using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.AppSettings;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/products")]
    public class ProductsController : CrudController<ProductDto>
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService, ILogService loggerService, IAppSettingsService appSettingsService) : base(productService, loggerService, appSettingsService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("cheapest")]
        public IHttpActionResult GetCheapest(decimal maxPrice) => Execute(() => _productService.GetCheapest(maxPrice));
    }
}
