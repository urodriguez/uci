using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/inventions")]
    public class InventionsController : CrudController<InventionDto>
    {
        private readonly IInventionService _inventionService;

        public InventionsController(IInventionService inventionService, ILogService loggerService, IInventAppContext inventAppContext) : base(inventionService, loggerService, inventAppContext)
        {
            _inventionService = inventionService;
        }

        [HttpGet]
        [Route("cheapest")]
        public async Task<IHttpActionResult> GetCheapest(decimal maxPrice) => await ExecuteAsync(async () => await _inventionService.GetCheapestAsync(maxPrice));
    }
}
