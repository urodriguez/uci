using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;

namespace WebApi.Controllers
{
    [Authorize]
    public class AuditsController : ApiController
    {
        private readonly IAuditService _auditService;

        public AuditsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok();
        }
    }
}