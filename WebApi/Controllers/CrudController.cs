using System;
using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Crosscutting;

namespace WebApi.Controllers
{
    public class CrudController<TDto> : InventAppApiController where TDto : IDto
    {
        private readonly ICrudService<TDto> _crudService;

        public CrudController(ICrudService<TDto> crudService, ILogService loggerService) : base (loggerService)
        {
            _crudService = crudService;
        }

        [HttpGet]
        public IHttpActionResult GetAll() => Execute(() => _crudService.GetAll());

        [HttpGet]
        public IHttpActionResult Get([FromUri] Guid id) => Execute(() => _crudService.GetById(id));

        [HttpPost]
        public IHttpActionResult Create([FromBody] TDto dto) => Execute(() => _crudService.Create(dto));

        [HttpPut]
        public IHttpActionResult Update([FromUri] Guid id, [FromBody] TDto dto) => Execute(() => _crudService.Update(id, dto));

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] Guid id) => Execute(() => _crudService.Delete(id));
    }
}