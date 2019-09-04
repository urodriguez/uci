using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CrudController<TDto> : ApiController where TDto : IDto
    {
        private readonly ICrudService<TDto> _crudService;
        private readonly ILogService _loggerService;

        public CrudController(ICrudService<TDto> crudService, ILogService loggerService)
        {
            _crudService = crudService;
            _loggerService = loggerService;
        }

        protected IHttpActionResult SendOk()
        {
            _loggerService.FlushQueueMessages();
            return Ok();
        }

        protected IHttpActionResult SendOk<T>(T data)
        {
            _loggerService.FlushQueueMessages();
            return Ok(data);
        }

        protected IHttpActionResult SendInternalServerError(Exception e)
        {
            _loggerService.QueueErrorMessage(e.ToString());
            _loggerService.FlushQueueMessages();

            return InternalServerError(e);
        }

        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var dtos = _crudService.GetAll();

                return SendOk(dtos);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri] Guid id)
        {
            try
            {
                var dto = _crudService.GetById(id);
                if (dto == null) return NotFound();

                return SendOk(dto);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] TDto dto)
        {
            try
            {
                var id = _crudService.Create(dto);
                return SendOk(id);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        [HttpPut]
        public IHttpActionResult Update([FromUri] Guid id, [FromBody] TDto dto)
        {
            try
            {
                _crudService.Update(id, dto);

                return SendOk();
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] Guid id)
        {
            try
            {
                _crudService.Delete(id);
            }
            catch (Exception e)
            {
                return SendInternalServerError(e);
            }

            return SendOk();
        }
    }
}