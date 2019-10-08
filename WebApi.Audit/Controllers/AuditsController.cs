using System.Collections.Generic;
using System.Linq;
using JsonDiffPatchDotNet;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApi.Audit.Domain.Enums;
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

            var objectChanges = GetChanges(auditDto.Entity, auditDto.OldEntity);
        }

        public class EntityChange
        {
            public string Field { get; set; }
            public string OldValue { get; set; }
            public string NewValue { get; set; }
        }

        public IEnumerable<EntityChange> GetChanges(string serializedEntity, string serializedOldEntity)
        {
            var entityObject = JObject.Parse(serializedEntity);
            var oldEntityObject = JObject.Parse(serializedOldEntity);

            var jdp = new JsonDiffPatch();
            if (jdp.Diff(oldEntityObject, JObject.Parse(@"{}")) == null)//serializedOldEntity is empty on AuditAction = Create
            {
                return entityObject.Properties().Select(ep => new EntityChange
                {
                    Field = ep.Path,
                    OldValue = "",
                    NewValue = ep.Value.ToString()
                });
            }

            var diffsObject = (JObject)jdp.Diff(entityObject, oldEntityObject);

            if (diffsObject == null) return new List<EntityChange>();//no changes on AuditAction = Update
            return diffsObject.Properties().Select(ep => new EntityChange
            {
                Field = ep.Path,
                OldValue = JsonConvert.DeserializeObject<List<string>>(ep.Value.ToString()).First(),
                NewValue = JsonConvert.DeserializeObject<List<string>>(ep.Value.ToString()).Last()
            });
        }
    }
}
