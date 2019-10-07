using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApi.Audit.Dtos;
using WebApi.Audit.Infrastructure.CrossCutting.Logging;

namespace WebApi.Audit.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditsController : ControllerBase
    {
        private readonly ILogService _logService;

        public AuditsController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            _logService.QueueInfoMessage($"AuditController - Getting entities audited");
            _logService.FlushQueueMessages();
            return new string[] { "audit1", "audit2" };
        }

        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "audit" + id;
        }

        [HttpPost]
        public void Audit([FromBody] AuditDto auditDto)
        {
            _logService.QueueInfoMessage($"AuditController - Auditing entity with id = {auditDto.EntityId}");
            _logService.FlushQueueMessages();

        }
    }
}
