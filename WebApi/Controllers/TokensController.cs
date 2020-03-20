using System.Web.Http;
using Application.Contracts.Services;
using Application.Dtos;
using Domain.Contracts.Infrastructure.Crosscutting.Logging;

namespace WebApi.Controllers
{
    public class TokensController : InventAppApiController
    {
        private readonly IUserService _userService;

        public TokensController(IUserService userService, ILogService logService) : base(logService)
        {
            _userService = userService;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody] CredentialsDto credentialsDto) => Execute(() => _userService.Login(credentialsDto));
    }
}