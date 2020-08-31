using System;
using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Infrastructure.Logging;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    public class CrudController<TDto> : InventAppApiController where TDto : ICrudDto
    {
        private readonly ICrudService<TDto> _crudService;

        public CrudController(ICrudService<TDto> crudService, ILogService loggerService, IInventAppContext inventAppContext) : base (loggerService, inventAppContext)
        {
            _crudService = crudService;
        }

        [HttpGet]
        public async Task<IHttpActionResult> GetAllAsync() => await ExecuteAsync(async () => await _crudService.GetAllAsync());

        [HttpGet]
        public async Task<IHttpActionResult> GetAsync([FromUri] Guid id) => await ExecuteAsync(async () => await _crudService.GetByIdAsync(id));

        [HttpPost]
        public async Task<IHttpActionResult> CreateAsync([FromBody] TDto dto) => await ExecuteAsync(async () => await _crudService.CreateAsync(dto));

        [HttpPut]
        public async Task<IHttpActionResult> UpdateAsync([FromUri] Guid id, [FromBody] TDto dto) => await ExecuteAsync(async () =>
        {
            dto.Id = id;
            return await _crudService.UpdateAsync(dto);
        });

        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([FromUri] Guid id) => await ExecuteAsync(async () => await _crudService.DeleteAsync(id));
    }
}