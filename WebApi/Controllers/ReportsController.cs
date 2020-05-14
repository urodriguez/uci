using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    [RoutePrefix("api/v1.0/reports")]
    public class ReportsController : InventAppApiController
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService, ILogService logService) : base(logService)
        {
            _reportService = reportService;
        }

        [HttpPost]
        [Route("products")]
        public IHttpActionResult CreateForProducts([FromBody] ReportProductDto reportProductDto) => Execute(() => _reportService.CreateForProducts(reportProductDto));
    }
}