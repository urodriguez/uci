using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    public class CrudController<TDto> : InventappApiController where TDto : IDto
    {
        private readonly ICrudService<TDto> _crudService;

        public CrudController(ICrudService<TDto> crudService, ILogService loggerService) : base (loggerService)
        {
            _crudService = crudService;
        }

        //[HttpGet]
        //public IHttpActionResult GetAll() => Execute(() => _crudService.GetAll());

        [HttpGet]
        public IHttpActionResult Get([FromUri] Guid id) => Execute(() => _crudService.GetById(id));

        [HttpPost]
        public IHttpActionResult Create([FromBody] TDto dto) => Execute(() => _crudService.Create(dto));

        [HttpPut]
        public IHttpActionResult Update([FromUri] Guid id, [FromBody] TDto dto)
        {
            return Execute(() =>
            {
                _crudService.Update(id, dto);
                return true;
            });
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri] Guid id)
        {
            return Execute(() =>
            {
                _crudService.Delete(id);
                return true;
            });
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync() => await ExecuteAsync(async () => await _crudService.GetAllAsync());
    }
}