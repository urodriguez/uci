using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;
using Application;
using Application.Adapters;
using Application.Dtos;
using Crosscutting.Logging;
using Persistence;

namespace WebApi.Controllers
{
  [EnableCors(origins: "*", headers: "*", methods: "*")]
  public class ProductsController : ApiController
  {
    private readonly IProductService _productService;
    private readonly ProductAdapter _productAdapter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerService _loggerService;

    public ProductsController(IProductService productService, ILoggerService loggerService)
    {
      //_unitOfWork = unitOfWork;
      _loggerService = loggerService;
      _productService = productService;
      //_productAdapter = new ProductAdapter();
    }


    [HttpGet]
    public IHttpActionResult GetAll()
    {
      _loggerService.QueueLogMessage(new LogMessage("GetAll method called", LogLevel.Trace));
      try
      {
        //var products = _productAdapter.AdaptBulk(_unitOfWork.ProductRepository.GetAll());
        //_loggerService.FlushQueueLogMessages();
        var products = _productService.GetAll();
        return Ok(products);
      }
      catch (Exception e)
      {
        //  _loggerService.QueueLogMessage(new LogMessage(e.Message, LogLevel.Error));
        //_loggerService.FlushQueueLogMessages();
        return InternalServerError();
      }
    }

    [HttpGet]
    public IHttpActionResult Get(int id)
    {
      _loggerService.QueueLogMessage(new LogMessage("Get method called", LogLevel.Trace));
      try
      {
        //var product = _productAdapter.Adapt(_unitOfWork.ProductRepository.GetById(id));
        var product = _productService.GetById(id);
        if (product == null) return NotFound();

        //_loggerService.QueueLogMessage(new LogMessage(JsonConvert.SerializeObject(product), LogLevel.Trace));
        //_loggerService.FlushQueueLogMessages();

        return Ok(product);
      }
      catch (Exception e)
      {
        //_loggerService.QueueLogMessage(new LogMessage(e.Message, LogLevel.Error));
        //_loggerService.FlushQueueLogMessages();
        return InternalServerError();
      }
    }

    [HttpPost]
    public IHttpActionResult Create([FromBody] ProductDto productDto)
    {
      //var product = _productAdapter.Adapt(productDto);
      //_unitOfWork.ProductRepository.Add(product);
      //_unitOfWork.SaveChanges();
      var productDtoCreated = _productService.Create(productDto);
      return Ok(productDtoCreated);
    }

    [HttpPut]
    public IHttpActionResult Update(int id, [FromBody] ProductDto productDto)
    {
      //var product = _productAdapter.Adapt(productDto);
      //_unitOfWork.ProductRepository.Update(product);
      //_unitOfWork.SaveChanges();
      var productDtoUpdated = _productService.Update(productDto);

      return Ok(productDtoUpdated);
    }

    [HttpDelete]
    public IHttpActionResult Delete(int id)
    {
      if (id <= 0) return BadRequest("Not a valid id");

      _unitOfWork.ProductRepository.Delete(id);
      _unitOfWork.SaveChanges();

      return Ok();
    }

    [HttpDelete]
    public IHttpActionResult Delete(string ids)
    {
      try
      {
        var idsList = ids.Split(';').Select(id => Convert.ToInt32(id));

        _unitOfWork.ProductRepository.DeleteBulk(idsList);
        _unitOfWork.SaveChanges();

        return Ok();
      }
      catch (Exception e)
      {
        return BadRequest("ids must be only positive integer numbers");
      }
    }
  }
}
