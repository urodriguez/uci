using System.Threading.Tasks;
using System.Web.Http;
using Application.Contracts;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/reports")]
    public class ReportsController : InventAppApiController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService, ILogService logService, IInventAppContext inventAppContext) : base(logService, inventAppContext)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Route("inventions")]
        public async Task<IHttpActionResult> CreateForInventionsAsync([FromBody] ReportInventionDto reportInventionDto) => await ExecuteAsync(async () => await _reportService.CreateForInventionsAsync(reportInventionDto));
    }
}