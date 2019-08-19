using System;
using System.Web.Http;
using System.Web.Http.Cors;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CrudController<TDto> : ApiController where TDto : IDto
    {
        private readonly ICrudService<TDto> _crudService;

        public CrudController(ICrudService<TDto> crudService)
        {
            _crudService = crudService;
        }


        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                var dtos = _crudService.GetAll();
                return Ok(dtos);
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
                var dto = _crudService.GetById(id);
                if (dto == null) return NotFound();

                return Ok(dto);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] TDto dto)
        {
            try
            {
                var id = _crudService.Create(dto);
                return Ok(id);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        public IHttpActionResult Update([FromUri] Guid id, [FromBody] TDto dto)
        {
            try
            {
                _crudService.Update(id, dto);

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
                _crudService.Delete(id);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }

            return Ok();
        }
    }
}